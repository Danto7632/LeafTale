using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class catMove : MonoBehaviour {
    private static catMove instance;

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

    private Queue<KeyValuePair<float, KeyCode>> keyPresses = new Queue<KeyValuePair<float, KeyCode>>();
    public float inputDelay = 1.0f; // 1초 안에 키 입력이 교차로 이루어져야 함

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

        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance != this) {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Ground");
    }

    void Update() {
        float currentSpeed = rb.velocity.x;

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
            Debug.DrawRay(groundRayVec, Vector2.down * 1f, Color.green);
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
        Debug.DrawRay(forwardVec, Vector2.right * ((isFacingRight == true) ? 1f : -1f), Color.blue, 0.3f); 

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
    }

    IEnumerator spawnDelay() {
        yield return new WaitForSeconds(0.5f);
        isMoveAllow = true;
    }
}

