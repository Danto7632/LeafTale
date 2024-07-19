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
    public float jumpForce = 12f;
    public float accelForce;
    public Vector3 newScale;
    public int UDCount;
    public int FlipCount;
    public int ClapCount;

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

    public bool isHandFlipped;

    public bool leftOn;

    private bool isClapDetected = false;

    public Text UDText;
    public Text FlipText;
    public Text ClapText;
    


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
        accelForce = 15f;

        isHandFlipped = false;

        leftOn = true;

        isClapDetected = false;

        UDCount = 0;
        FlipCount = 0;
        ClapCount = 0;

        UDText = GameObject.Find("UDText").GetComponent<Text>();
        FlipText = GameObject.Find("FlipText").GetComponent<Text>();
        ClapText = GameObject.Find("ClapText").GetComponent<Text>();
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
                if(isFacingRight) {
                    rb.velocity = new Vector2(accelForce, rb.velocity.y);
                }
                else if(!isFacingRight){
                    rb.velocity = new Vector2(-accelForce, rb.velocity.y);
                }

                keyPresses.Clear(); // 패턴이 완료되면 큐를 비웁니다.
            }
        }

        UDText.text = "Run : " + UDCount;
        FlipText.text = "Jump : " + FlipCount;
        ClapText.text = "Flip : " + ClapCount; 
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
            isGrounded = false;
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
            GameManager.instance.AddScore(2);
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

                if (handSpeed > 5f && !isClapDetected) {
                    if (hand.IsLeft && leftOn) {
                        if (Mathf.Abs(handDirection.y) > Mathf.Abs(handDirection.x)) {
                            if (handDirection.y > 0) {

                                if(isFacingRight && isMoveAllow) {
                                    rb.velocity = new Vector2(accelForce, rb.velocity.y);
                                }
                                else if(!isFacingRight && isMoveAllow){
                                    rb.velocity = new Vector2(-accelForce, rb.velocity.y);
                                }

                                UDCount++;
                            }
                        }
                        leftOn = false;
                    }

                    if (hand.IsRight && !leftOn) {
                        if (Mathf.Abs(handDirection.y) > Mathf.Abs(handDirection.x)) {
                            if (handDirection.y > 0) {
                                if(isFacingRight && isMoveAllow) {
                                    rb.velocity = new Vector2(accelForce, rb.velocity.y);
                                }
                                else if(!isFacingRight && isMoveAllow){
                                    rb.velocity = new Vector2(-accelForce, rb.velocity.y);
                                }
                            }
                            UDCount++;
                        }
                        leftOn = true;
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
    }

    void catJumpFlip(Frame frame) {
        foreach (Hand hand in frame.Hands) {
            if (hand.IsLeft) { // 왼손만 감지하도록 조건 추가
                Vector3 palmNormal = hand.PalmNormal;

                if (isGrounded && isMoveAllow) {
                    if (palmNormal.y > 0.5f && !isHandFlipped) {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                        isHandFlipped = true;
                        FlipCount++;
                    }    
                    else if (palmNormal.y < -0.5f && isHandFlipped) {
                        isHandFlipped = false;
                    }
                }

                break; // 한 손만 감지한 후 루프 종료
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

                rb.velocity = new Vector2(0, rb.velocity.y);
                ClapCount++;
                isClapDetected = true; // 손이 맞대어졌음을 감지
            }
            else if (handsDistance >= clapDistanceThreshold && isClapDetected) {
            // 손이 맞대어졌다가 떨어짐을 감지
                isClapDetected = false; // 상태 초기화
            }
        }
    }

    #endregion
}

