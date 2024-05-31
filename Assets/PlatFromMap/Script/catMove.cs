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
    public float Hp;
    public float moveSpeed = 10f;
    public float jumpForce = 12f;

    [Header("Player_Component")]
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public CapsuleCollider2D capsule2D;
    public Animator anim;

    [Header("Player_Condition")]
    public RaycastHit2D[] isGroundeds = new RaycastHit2D[17];
    public bool isGrounded;
    public bool isCoyoteLeft;
    public bool isCoyoteRight;
    public bool isFacingRight;
    public bool isMoveAllow;

    [Header("RayCast")]
    Vector2 moveDirection;
    Vector2 groundRayVec;
    Vector2 wallRayVec;
    Vector2 CoyoteLeftVec;
    Vector2 CoyoteRightVec;
    public float groundRayThickness;
    public float wallRayThickness;
    public int groundRayCount;

    [Header("Layer")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private bool isPaused = false;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        isMoveAllow = true;
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
        if(Input.GetKeyDown(KeyCode.M)) {
            GamePuased();
        }
        
        Jump();
        Flip();
    }

    void FixedUpdate() {
        isPlayerGround();
        catAnim();

        if(isFacingRight) {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }

    void isPlayerGround() {
        groundRayThickness = -0.8f;
        for(int i = 0; i < groundRayCount; i++) {
            groundRayVec = new Vector2(transform.position.x + groundRayThickness, transform.position.y - 1.0f);
            isGroundeds[i] = Physics2D.Raycast(groundRayVec, Vector2.down, 0.01f, groundLayer);
            Debug.DrawRay(groundRayVec, Vector2.down * 0.01f, Color.green);
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
        }
        else {
            moveSpeed = 10f;
        }

        if(forwardHit.collider == null) {
            CoyoteLeftVec = new Vector2(this.transform.position.x - 0.8f, this.transform.position.y);
            isCoyoteLeft = Physics2D.Raycast(CoyoteLeftVec, Vector2.down, 0.51f, groundLayer);
            Debug.DrawRay(CoyoteLeftVec, Vector2.down * 0.51f, Color.green);
            if(isCoyoteLeft) {
                isGrounded = true;
            }
            else {
                isGrounded = false;
            }

            CoyoteRightVec = new Vector2(this.transform.position.x + 0.8f, this.transform.position.y);
            isCoyoteRight = Physics2D.Raycast(CoyoteRightVec, Vector2.down, 0.51f, groundLayer);
            Debug.DrawRay(CoyoteRightVec, Vector2.down * 0.51f, Color.green);
            if(isCoyoteRight) {
                isGrounded = true;
            }
            else {
                isGrounded = false;
            }
        }
    }

    #region PlayerMove

    void Jump() {
        if(isGrounded && Input.GetButtonDown("Jump") && isMoveAllow) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void Flip() {
        if(isFacingRight && Input.GetKeyDown(KeyCode.LeftArrow)) {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            isFacingRight = false;
        }
        else if(!isFacingRight && Input.GetKeyDown(KeyCode.RightArrow)) {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            isFacingRight = true;
        }
    }

    #endregion  

    public void GamePuased() {
        if(isPaused) {
            Time.timeScale = 1f;
            isPaused = false;
        }
        else {
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void catAnim() {
        anim.SetBool("isGround", isGrounded);
    }
}