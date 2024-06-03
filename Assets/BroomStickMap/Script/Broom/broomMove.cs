using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class broomMove : MonoBehaviour {
    [Header("Player_Status")]
    public float horiaontalInput;
    public float verticalInput;
    public float moveSpeed;
    public float basicSpeed;

    [Header("Player_Component")]
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public CapsuleCollider2D capsule2D;
    public Animator anim;

    [Header("Player_Condition")]
    public bool isHit;
    public Vector2 moveDirection;

    void Awake() {
        moveSpeed = 8f;
        basicSpeed = 10f;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        lineControl();
    }

    void lineControl() {
        horiaontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector2(horiaontalInput, verticalInput);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, (moveDirection.y * moveSpeed) + basicSpeed);
    }
}
