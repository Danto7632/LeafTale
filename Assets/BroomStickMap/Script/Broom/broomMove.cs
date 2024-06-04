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
    public bool isHit;
    public Vector2 moveDirection;

    [Header("Movement_Limits")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    GameObject hp;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        moveSpeed = 8f;

        minX = -6.5f;
        maxX = 6.5f;
        minY = -3f;
        maxY = 3f;

        hp = GameObject.Find("Heart");
    }

    void Update() {
        lineControl();
    }

    void lineControl() {
        horiaontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if(!isHit) {
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
            hp.GetComponent<HeartUi>().SetHp(-1);
            StartCoroutine(hitDelay());
        }
    }

    IEnumerator hitDelay() {
        isHit = true;
        rb.velocity = Vector2.zero;

        this.gameObject.transform.position = new Vector2(0, 0);

        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0f);


        yield return new WaitForSeconds(1.5f);

        isHit = false;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);
    }
}
