using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Leap;
using Leap.Unity;

public class catMove : MonoBehaviour {
    [Header("Player_Status")]
    public float horiaontalInput;
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public float accelForce = 2f;
    public float accelMaxForce = 8f;
    public Vector3 newScale;

    [Header("Player_Component")]
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public CapsuleCollider2D capsule2D;
    public Animator anim;
    float currentSpeed;

    [Header("Player_Condition")]
    public RaycastHit2D[] isGroundeds = new RaycastHit2D[17];
    public bool isGrounded;
    public bool isFacingRight;
    public bool isMoveAllow;
    public bool isGameOver;

    [Header("RayCast")]
    Vector2 moveDirection;
    Vector2 groundRayVec;
    Vector2 wallRayVec;
    public float groundRayThickness;
    public float wallRayThickness;
    public int groundRayCount;

    [Header("Layer")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    
    [Header("LeapMotion")]
    private LeapServiceProvider leapProvider;
    private Vector3 previousLeftHandPosition = Vector3.zero;
    private Vector3 previousRightHandPosition = Vector3.zero;

    private bool isLeftMovingUp = false;
    private bool isLeftMovingDown = false;
    private bool isRightMovingUp = false;
    private bool isRightMovingDown = false; 

    public bool isHandFlipped;

    private bool shouldPrintLURD = false; // 왼손이 위로, 오른손이 아래로 움직임을 출력해야 할지 여부
    private bool shouldPrintLDRU = false; // 왼손이 아래로, 오른손이 위로 움직임을 출력해야 할지 여부

    private bool isClapDetected = false;


    private Queue<KeyValuePair<float, KeyCode>> keyPresses = new Queue<KeyValuePair<float, KeyCode>>();
    public float inputDelay = 1.0f;

    void Awake() {
        isMoveAllow = false;
        isGameOver = false;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        isMoveAllow = false;
        groundRayCount = 17;
        isFacingRight = true;
        anim.SetBool("isGround", true);
        accelForce = 10f;

        isLeftMovingUp = false;
        isLeftMovingDown = false;
        isRightMovingUp = false;
        isRightMovingDown = false; 

        isHandFlipped = false;

        shouldPrintLURD = false; // 왼손이 위로, 오른손이 아래로 움직임을 출력해야 할지 여부
        shouldPrintLDRU = false; // 왼손이 아래로, 오른손이 위로 움직임을 출력해야 할지 여부

        isClapDetected = false;
    }

    void Start() {
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Ground");

        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }

    void Update() {
        currentSpeed = rb.velocity.x;

        Jump();
        Flip();

        if(isMoveAllow) {
            DetectKeyPress(KeyCode.R);
            DetectKeyPress(KeyCode.T);

            if (IsPatternComplete()) {
                if(isFacingRight && (currentSpeed < accelMaxForce)) {
                    rb.AddForce(new Vector2(accelForce, 0), ForceMode2D.Impulse);
                }
                else if(!isFacingRight && (currentSpeed > -accelMaxForce)){
                    rb.AddForce(new Vector2(-accelForce, 0), ForceMode2D.Impulse);
                }

                keyPresses.Clear(); // 패턴이 완료되면 큐를 비웁니다.
            }
        }
    }

    void DetectKeyPress(KeyCode key) {
        if (Input.GetKeyDown(key)) {
            keyPresses.Enqueue(new KeyValuePair<float, KeyCode>(Time.time, key));
            CleanUpOldKeyPresses();
        }
    }

    void CleanUpOldKeyPresses() {
        while (keyPresses.Count > 0 && (Time.time - keyPresses.Peek().Key > inputDelay)) {
            keyPresses.Dequeue();
        }
    }

    bool IsPatternComplete() {
        if (keyPresses.Count < 2) { // R과 T를 교차로 두 번 이상 입력해야 하므로, 패턴의 길이가 2 이상인지 확인합니다.
            return false;
        }

        KeyValuePair<float, KeyCode>[] keyPressArray = keyPresses.ToArray();
        int patternIndex = 0;

        foreach (var keyPress in keyPressArray) {
            if (keyPress.Value == KeyCode.R && patternIndex % 2 == 0) { // R과 T가 교차로 입력되어야 하므로, 인덱스가 짝수일 때 R 키를 확인합니다.
                patternIndex++;
            }
            else if (keyPress.Value == KeyCode.T && patternIndex % 2 == 1) { // R과 T가 교차로 입력되어야 하므로, 인덱스가 홀수일 때 T 키를 확인합니다.
                patternIndex++;
            }

            if (patternIndex >= 2) { // R과 T를 각각 하나씩 입력해야 하므로, 패턴이 2 이상일 때 완료된 것으로 간주합니다.
                return true;
            }
        }

        return false;
    }

    void FixedUpdate() {
        isPlayerGround();
        catAnim();
    }

    void isPlayerGround() {
        groundRayThickness = -0.8f;
        for(int i = 0; i < groundRayCount; i++) {
            groundRayVec = new Vector2(transform.position.x + groundRayThickness, transform.position.y);
            isGroundeds[i] = Physics2D.Raycast(groundRayVec, Vector2.down, 1f, groundLayer);
            if(isGroundeds[i].collider != null) {
                isGrounded = true;
                break;
            }
            else {
                isGrounded = false;
            }
            groundRayThickness += 0.1f;
        }

        Vector2 forwardVec = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D forwardHit = Physics2D.Raycast(forwardVec, Vector2.right * ((isFacingRight == true) ? 1f : -1f), 1.0f, wallLayer);

        if(forwardHit.collider != null) {
            moveSpeed = 0f;
            isGrounded = false;
        }
        else {
            moveSpeed = 10f;
        }
    }

    #region PlayerMove

    void Jump() {
        if(isGrounded && Input.GetButtonDown("Jump") && isMoveAllow) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void Flip() {
        if(isFacingRight && Input.GetKeyDown(KeyCode.LeftArrow) && isMoveAllow) {
            newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            isFacingRight = false;
        }
        else if(!isFacingRight && Input.GetKeyDown(KeyCode.RightArrow) && isMoveAllow) {
            newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            isFacingRight = true;
        }
    }

    #endregion

    public void catAnim() {
        anim.SetBool("isReady", !isMoveAllow);
        if(rb.velocity.x == 0 && isMoveAllow) {
            anim.SetBool("isIdle", true);
        }
        else if(rb.velocity.x != 0 && isMoveAllow) {
            anim.SetBool("isIdle", false);
            anim.SetBool("isGround", isGrounded);
        }
        anim.SetBool("isGameOver", isGameOver);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("EndZone")) {
            isMoveAllow = false;
            isGameOver = true;
            rb.velocity = Vector2.zero;
        }

        if(other.gameObject.CompareTag("SpawnZone")) {
            isMoveAllow = false;
            rb.velocity = Vector2.zero;

            StartCoroutine(spawnDelay());
        }

        if(other.gameObject.CompareTag("Coin")) // 코인 먹었을 때
        {
            other.gameObject.SetActive(false); // 코인 사라짐
        }
    }

    IEnumerator spawnDelay() {
        yield return new WaitForSeconds(0.5f);
        isMoveAllow = true;
    }


    #region LeapMotion

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void OnUpdateFrame(Frame frame) {
        if(isMoveAllow) {
            catMovingUPDOWN(frame);
        }
        catJumpFlip(frame);
        catFlipClap(frame);
    }

    void catMovingUPDOWN(Frame frame) {
        foreach (Hand hand in frame.Hands) {
            Vector3 currentHandPosition = hand.PalmPosition;
            Vector3 previousHandPosition = hand.IsLeft ? previousLeftHandPosition : previousRightHandPosition;

            if (previousHandPosition != Vector3.zero) {
                Vector3 handDirection = (currentHandPosition - previousHandPosition).normalized;
                float handSpeed = hand.PalmVelocity.magnitude;

                if (handSpeed > 4f) {
                    if (hand.IsLeft) {
                        if (Mathf.Abs(handDirection.y) > Mathf.Abs(handDirection.x)) {
                            if (handDirection.y > 0) {
                                isLeftMovingUp = true; // 왼손이 위로
                                isLeftMovingDown = false;
                            }
                            else {
                                isLeftMovingDown = true; // 왼손이 아래로
                                isLeftMovingUp = false;
                            }
                        }
                    }
                    else if (hand.IsRight) {
                        if (Mathf.Abs(handDirection.y) > Mathf.Abs(handDirection.x)) {
                            if (handDirection.y > 0) {
                                isRightMovingUp = true; // 오른손이 위로
                                isRightMovingDown = false;
                            }
                            else {
                                isRightMovingDown = true; // 오른손이 아래로
                                isRightMovingUp = false;
                            }
                        }
                    }
                }
            }
            else {
                if (hand.IsLeft) {
                    previousLeftHandPosition = currentHandPosition;
                }
                else if (hand.IsRight) {
                    previousRightHandPosition = currentHandPosition;
                }
            }
        }

        // 번갈아가며 움직일 때 콘솔 출력
        if (isLeftMovingUp && isRightMovingDown && !shouldPrintLURD) {
            Debug.Log("왼손이 위로, 오른손이 아래로 움직임");
            shouldPrintLURD = true;
            shouldPrintLDRU = false;

            if(isFacingRight && (currentSpeed < accelMaxForce) && isMoveAllow) {
                rb.AddForce(new Vector2(accelForce, 0), ForceMode2D.Impulse);
            }
            else if(!isFacingRight && (currentSpeed > -accelMaxForce) && isMoveAllow){
                rb.AddForce(new Vector2(-accelForce, 0), ForceMode2D.Impulse);
            }
        }
        else if (isLeftMovingDown && isRightMovingUp && !shouldPrintLDRU) {
            Debug.Log("왼손이 아래로, 오른손이 위로 움직임");
            shouldPrintLURD = false;
            shouldPrintLDRU = true;

            if(isFacingRight && (currentSpeed < accelMaxForce)) {
                rb.AddForce(new Vector2(accelForce, 0), ForceMode2D.Impulse);
            }
            else if(!isFacingRight && (currentSpeed > -accelMaxForce)){
                rb.AddForce(new Vector2(-accelForce, 0), ForceMode2D.Impulse);
            }
        }

        // 움직임이 멈추면 상태 초기화
        if (!(isLeftMovingUp || isLeftMovingDown)) {
            shouldPrintLURD = false;
        }
        if (!(isRightMovingUp || isRightMovingDown)) {
            shouldPrintLDRU = false;
        }

        // 모든 움직임 상태 초기화
        isLeftMovingUp = false;
        isLeftMovingDown = false;
        isRightMovingUp = false;
        isRightMovingDown = false;
    }

    void catJumpFlip(Frame frame) {
        foreach (Hand hand in frame.Hands) {
            Vector3 palmNormal = hand.PalmNormal;

            if(isGrounded && isMoveAllow) {
                if (palmNormal.y > 0.5f && !isHandFlipped) {
                    // 손바닥이 위로 향하게 된 상태
                    isHandFlipped = true;

                    Debug.Log("Left hand palm facing up");
                }
                else if (palmNormal.y < -0.5f && isHandFlipped) {
                    isHandFlipped = false;
                    // 손바닥이 다시 아래로 향하게 된 상태
                    Debug.Log("Left hand palm facing down again");
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }
        }
    }

    void catFlipClap(Frame frame) {
        Hand leftHand = null;
        Hand rightHand = null;

        foreach (Hand hand in frame.Hands) {
            if (hand.IsLeft)
                leftHand = hand;
            else if (hand.IsRight)
                rightHand = hand;
        }

        if (leftHand != null && rightHand != null) {
            Vector3 leftPalmPosition = leftHand.PalmPosition;
            Vector3 rightPalmPosition = rightHand.PalmPosition;

            float clapDistanceThreshold = 0.5f;
            float handsDistance = Vector3.Distance(leftPalmPosition, rightPalmPosition);

            if (handsDistance < clapDistanceThreshold && !isClapDetected) {
                isClapDetected = true; // 손이 맞대어졌음을 감지
            }
            else if (handsDistance >= clapDistanceThreshold && isClapDetected) {
            // 손이 맞대어졌다가 떨어짐을 감지
                if (isFacingRight && isMoveAllow) {
                    newScale = transform.localScale;
                    newScale.x *= -1;
                    transform.localScale = newScale;
                    isFacingRight = false;
                }
                else if (!isFacingRight && isMoveAllow) {
                    newScale = transform.localScale;
                    newScale.x *= -1;
                    transform.localScale = newScale;
                    isFacingRight = true;
                }

                isClapDetected = false; // 상태 초기화
            }
        }
    }

    #endregion
}

