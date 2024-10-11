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
    public float jumpForce;
    public float accelForce;
    public float currentSpeed;
    public int UDCount;
    public int FlipCount;
    public int ClapCount;
    public Vector3 newScale;

    [Header("Player_Component")]
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public CapsuleCollider2D capsule2D;
    public Animator anim;
    public platSoundManager psm;

    [Header("Player_Condition")]
    public RaycastHit2D[] isGroundeds;
    public bool isGrounded;
    public bool isFacingRight;
    public bool isMoveAllow;
    public bool isGameOver;

    [Header("RayCast")]
    public Vector2 moveDirection;
    public Vector2 groundRayVec;
    public Vector2 wallRayVec;
    public float groundRayThickness;
    public float wallRayThickness;
    public int groundRayCount;

    [Header("Layer")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    
    [Header("LeapMotion")]
    public LeapServiceProvider leapProvider;
    public Vector3 previousLeftHandPosition = Vector3.zero;
    public Vector3 previousRightHandPosition = Vector3.zero;
    public bool isHandFlipped;
    public bool leftOn;
    public bool isClapDetected;
    public Text UDText;
    public Text FlipText;
    public Text ClapText;
    
    private Queue<KeyValuePair<float, KeyCode>> keyPresses = new Queue<KeyValuePair<float, KeyCode>>();
    public float inputDelay;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        UDText = GameObject.Find("UDText").GetComponent<Text>();
        FlipText = GameObject.Find("FlipText").GetComponent<Text>();
        ClapText = GameObject.Find("ClapText").GetComponent<Text>();
        psm = GameObject.Find("SoundManager").GetComponent<platSoundManager>();

        anim.SetBool("isGround", true);

        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Ground");

        isGroundeds = new RaycastHit2D[17];

        isFacingRight = true;
        isMoveAllow = false;
        isGameOver = false;
        isHandFlipped = false;
        leftOn = true;
        isClapDetected = false;

        groundRayCount = 17;
        jumpForce = 12f;
        accelForce = 15f;
        inputDelay = 1.0f;

        UDCount = 0;
        FlipCount = 0;
        ClapCount = 0;

        previousLeftHandPosition = Vector3.zero;
        previousRightHandPosition = Vector3.zero;

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
                if(isFacingRight) rb.velocity = new Vector2(accelForce, rb.velocity.y);
                else if(!isFacingRight) rb.velocity = new Vector2(-accelForce, rb.velocity.y);
                psm.moveSound.Play();
                keyPresses.Clear();
            }
        }

        UDText.text = "Run : " + UDCount;
        FlipText.text = "Jump : " + FlipCount;
        ClapText.text = "Flip : " + ClapCount; 
    }

    void FixedUpdate() {
        isPlayerGround();
        catAnim();
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
        if (keyPresses.Count < 2) return false;

        KeyValuePair<float, KeyCode>[] keyPressArray = keyPresses.ToArray();
        int patternIndex = 0;

        foreach (var keyPress in keyPressArray) {
            if (keyPress.Value == KeyCode.R && patternIndex % 2 == 0) patternIndex++;
            else if (keyPress.Value == KeyCode.T && patternIndex % 2 == 1) patternIndex++;
            if (patternIndex >= 2) return true;
        }
        return false;
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

        if(forwardHit.collider != null) isGrounded = false;
    }

    void Jump() {
        if(isGrounded && Input.GetButtonDown("Jump") && isMoveAllow) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            psm.jumpSound.Play();
        }
    }

    void Flip() {
        if(isFacingRight && Input.GetKeyDown(KeyCode.LeftArrow) && isMoveAllow) {
            newScale = transform.localScale;
            newScale.x *= -1;

            transform.localScale = newScale;
            isFacingRight = false;
            psm.flipSound.Play();
        }
        else if(!isFacingRight && Input.GetKeyDown(KeyCode.RightArrow) && isMoveAllow) {
            newScale = transform.localScale;
            newScale.x *= -1;

            transform.localScale = newScale;
            isFacingRight = true;
            psm.flipSound.Play();
        }
    }

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
            if(StoryOrStage.instance != null) {
                StoryOrStage.instance.isPlatGood = true;
                StoryOrStage.instance.clearCount++;
            }
            isMoveAllow = false;
            isGameOver = true;
            rb.velocity = Vector2.zero;
        }

        if(other.gameObject.CompareTag("SpawnZone")) {
            isMoveAllow = false;
            rb.velocity = Vector2.zero;
            StartCoroutine(spawnDelay());
            psm.spawnSound.Play();
        }
        
        if(other.gameObject.CompareTag("Coin")) {
            GameManager.instance.AddScore(2);
            other.gameObject.SetActive(false);
            psm.coinSound.Play();
        }
    }

    IEnumerator spawnDelay() {
        yield return new WaitForSeconds(0.5f);

        isMoveAllow = true;
    }


    #region LeapMotion

    void OnDestroy() {
        if (leapProvider != null) leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame) {
        if(isMoveAllow) {
            catMovingUPDOWN(frame);
        }

        catJumpFlip(frame);
        catFlipClap(frame);
    }

    void catMovingUPDOWN(Frame frame) {
        // 프레임에 있는 모든 손에 대해 반복문 실행
        foreach (Hand hand in frame.Hands) {
            // 손바닥의 현재 위치를 3D 좌표로 저장
            Vector3 currentHandPosition = hand.PalmPosition;
            // 왼손인지 오른손인지에 따라 이전 손바닥 위치를 설정
            Vector3 previousHandPosition = hand.IsLeft ? previousLeftHandPosition : previousRightHandPosition;

            // 이전 손 위치가 초기화된 상태가 아닌 경우
            if (previousHandPosition != Vector3.zero) {
                // 현재 손 위치와 이전 손 위치의 차이를 통해 이동 방향 계산
                Vector3 handDirection = (currentHandPosition - previousHandPosition).normalized;
                // 손의 속도를 계산
                float handSpeed = hand.PalmVelocity.magnitude;

                // 손의 속도가 일정 속도 이상이고 박수 탐지가 되지 않은 경우
                if (handSpeed > 5f && !isClapDetected) {
                    // 왼손이 활성화된 상태인 경우
                    if (hand.IsLeft && leftOn) {
                        // 수직 방향의 움직임이 수평 방향보다 큰 경우
                        if (Mathf.Abs(handDirection.y) > Mathf.Abs(handDirection.x)) {
                            // 손이 위쪽으로 움직인 경우
                            if (handDirection.y > 0) {
                                // 오른쪽을 향하고 있고 움직임이 허용된 경우
                                if(isFacingRight && isMoveAllow) rb.velocity = new Vector2(accelForce, rb.velocity.y);
                                // 왼쪽을 향하고 있고 움직임이 허용된 경우
                                else if(!isFacingRight && isMoveAllow) rb.velocity = new Vector2(-accelForce, rb.velocity.y);

                                // 위아래 움직임 카운트를 증가시킴
                                UDCount++;
                                psm.moveSound.Play();
                            }
                        }

                        // 왼손 비활성화
                        leftOn = false;
                    }

                    // 오른손이 활성화된 상태인 경우
                    if (hand.IsRight && !leftOn) {
                        // 수직 방향의 움직임이 수평 방향보다 큰 경우
                        if (Mathf.Abs(handDirection.y) > Mathf.Abs(handDirection.x)) {
                            // 손이 위쪽으로 움직인 경우
                            if (handDirection.y > 0) {
                                // 오른쪽을 향하고 있고 움직임이 허용된 경우
                                if(isFacingRight && isMoveAllow) rb.velocity = new Vector2(accelForce, rb.velocity.y);
                                // 왼쪽을 향하고 있고 움직임이 허용된 경우
                                else if(!isFacingRight && isMoveAllow) rb.velocity = new Vector2(-accelForce, rb.velocity.y);
                            }

                            // 위아래 움직임 카운트를 증가시킴
                            UDCount++;
                            psm.moveSound.Play();
                        }

                        // 오른손 비활성화
                        leftOn = true;
                    }
                }
            }

            // 이전 손 위치가 초기화된 상태인 경우 현재 손 위치로 설정
            else {
                if (hand.IsLeft) previousLeftHandPosition = currentHandPosition;
                else if (hand.IsRight) previousRightHandPosition = currentHandPosition;
            }

        }
    }

    void catJumpFlip(Frame frame) {
        // 프레임에 있는 모든 손에 대해 반복문 실행
        foreach (Hand hand in frame.Hands) {
            // 왼손일 경우 실행
            if (hand.IsLeft) {
                // 손바닥의 법선을 3D 좌표로 저장
                Vector3 palmNormal = hand.PalmNormal;

                // 객체가 땅에 있고 움직임이 허용된 경우
                if (isGrounded && isMoveAllow) {
                    // 손바닥이 위를 향하고 있고 손이 뒤집히지 않은 경우
                    if (palmNormal.y > 0.5f && !isHandFlipped) {
                        // 객체에게 점프 속도를 부여
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                        isHandFlipped = true; // 손이 뒤집힌 상태로 설정
                        psm.jumpSound.Play();
                        FlipCount++; // 뒤집기 횟수 증가
                    }
                    // 손바닥이 아래를 향하고 있고 손이 뒤집힌 상태인 경우
                    else if (palmNormal.y < -0.5f && isHandFlipped) {
                        isHandFlipped = false; // 손이 뒤집히지 않은 상태로 설정
                    }
                }

                // 왼손을 발견하면 반복문 종료
                break;
            }
        }
    }

    void catFlipClap(Frame frame) {
        Hand leftHand = null; // 왼손을 저장할 변수 초기화
        Hand rightHand = null; // 오른손을 저장할 변수 초기화

        // 프레임에 있는 모든 손에 대해 반복문 실행
        foreach (Hand hand in frame.Hands) {
            // 왼손일 경우 leftHand 변수에 저장
            if (hand.IsLeft) leftHand = hand;
            // 오른손일 경우 rightHand 변수에 저장
            else if (hand.IsRight) rightHand = hand;
        }

        // 왼손과 오른손이 모두 존재할 경우
        if (leftHand != null && rightHand != null) {
            // 왼손과 오른손의 손바닥 위치를 3D 좌표로 저장
            Vector3 leftPalmPosition = leftHand.PalmPosition;
            Vector3 rightPalmPosition = rightHand.PalmPosition;

            float clapDistanceThreshold = 0.5f; // 박수 인식 거리 임계값
            // 왼손과 오른손 사이의 거리를 계산
            float handsDistance = Vector3.Distance(leftPalmPosition, rightPalmPosition);

            // 손 사이의 거리가 임계값보다 작고 박수가 인식되지 않은 경우
            if (handsDistance < clapDistanceThreshold && !isClapDetected) {
                // 오른쪽을 향하고 있고 움직임이 허용된 경우
                if (isFacingRight && isMoveAllow) {
                    // 객체의 스케일을 반전시켜 왼쪽으로 방향을 변경
                    newScale = transform.localScale;
                    newScale.x *= -1;

                    transform.localScale = newScale;
                    isFacingRight = false;
                }
                // 왼쪽을 향하고 있고 움직임이 허용된 경우
                else if (!isFacingRight && isMoveAllow) {
                    // 객체의 스케일을 반전시켜 오른쪽으로 방향을 변경
                    newScale = transform.localScale;
                    newScale.x *= -1;

                    transform.localScale = newScale;
                    isFacingRight = true;
                }

                // 객체의 수평 속도를 0으로 설정하여 정지
                rb.velocity = new Vector2(0, rb.velocity.y);
                psm.flipSound.Play();
                ClapCount++; // 박수 횟수 증가

                isClapDetected = true; // 박수 인식 상태로 설정
            }
            // 손 사이의 거리가 임계값보다 크고 박수가 인식된 상태인 경우
            else if (handsDistance >= clapDistanceThreshold && isClapDetected) {
                isClapDetected = false; // 박수 인식 상태 해제
            }
        }
    }

    #endregion
}

