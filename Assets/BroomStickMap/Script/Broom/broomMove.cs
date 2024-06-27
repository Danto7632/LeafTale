using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class broomMove : MonoBehaviour {

    [Header("Player_Status")]
    public float horiaontalInput;
    public float verticalInput;
    public float moveSpeed;

    [Header("Player_Component")]
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public CapsuleCollider2D capsule2D;
    public Animator anim;

    [Header("Player_Condition")]
    public int Hp;
    public bool isHit;
    public bool isMoveAllow;
    public bool isGameOver;
    public bool isGameClear;
    public Vector2 moveDirection;

    [Header("Movement_Limits")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        moveSpeed = 8f;
        Hp = 5;

        isHit = false;
        isGameOver = false;
        isGameClear = false;

        minX = -6.5f;
        maxX = 6.5f;
        minY = -3f;
        maxY = 3f;
    }

    void Update() {
        lineControl();

        if(isGameClear) {
            isMoveAllow = false;
        }
    }

    void lineControl() {
        horiaontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if(!isHit && isMoveAllow) {
            moveDirection = new Vector2(horiaontalInput, verticalInput);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }

        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        rb.position = clampedPosition;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("BroomEnemy") && !isHit) {
            StartCoroutine(hitDelay());
            Debug.Log("Hit");
        }
    }

    IEnumerator hitDelay() {
        Debug.Log("Really Hit");
        Hp--;
        if(Hp <= 0) {
            isGameOver = true;
            isMoveAllow = false;

            this.gameObject.transform.position = new Vector2(0, 0);

            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0f);

            yield break;
        }

        isHit = true;
        rb.velocity = Vector2.zero;

        this.gameObject.transform.position = new Vector2(0, -3);

        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0f);


        yield return new WaitForSeconds(1.5f);

        isHit = false;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);
    }
}
