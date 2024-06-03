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


    private float lastKeyPressTime = 0f;
    private float keyPressInterval = 0.2f; // Adjust the interval as needed
    private float speedIncrement = 5f;

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

        if (isMoveAllow) {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.T)) {
                float currentTime = Time.time;
                if (currentTime - lastKeyPressTime <= keyPressInterval) {
                    if (isFacingRight && (currentSpeed < accelMaxForce)) {
                        rb.AddForce(new Vector2(accelForce + speedIncrement, 0), ForceMode2D.Impulse);
                    } else if (!isFacingRight && (currentSpeed > -accelMaxForce)) {
                        rb.AddForce(new Vector2(-accelForce - speedIncrement, 0), ForceMode2D.Impulse);
                    }
                    speedIncrement += 1f; // Increment the speed
                } else {
                    // Reset the speed increment if the interval is too long
                    speedIncrement = 1f;
                }
                lastKeyPressTime = currentTime;
            }
        }
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
            isMoveAllow = false;
            isGrounded = false;
        }
        else {
            isMoveAllow = true;
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
        if (other.gameObject.CompareTag("EndZone")) {
            isMoveAllow = false;
            isGameOver = true;
            rb.velocity = Vector2.zero;
        }

        if (other.gameObject.CompareTag("SpawnZone")) {
            isMoveAllow = false;
            rb.velocity = Vector2.zero;
            StartCoroutine(spawnDelay()); // Start the coroutine here
        }
    }

    IEnumerator spawnDelay() {
        yield return new WaitForSeconds(0.5f);
        isMoveAllow = true;
    }
}