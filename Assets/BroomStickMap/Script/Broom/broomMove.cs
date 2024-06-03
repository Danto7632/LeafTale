using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class broomMove : MonoBehaviour {
    [Header("Player_Status")]
    public int lineCount;
    public float moveSpeed = 10f;
    public Vector3 newScale;

    [Header("Player_Component")]
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public CapsuleCollider2D capsule2D;
    public Animator anim;

    [Header("Player_Condition")]
    public bool isHit;

    void Awake() {
        lineCount = 0;

        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start() {
        
    }

    void Update() {
        rb.velocity = new Vector2(0, moveSpeed);
        lineControl();
    }

    void lineControl() {
        if(Input.GetKeyDown(KeyCode.LeftArrow) && lineCount >= 0) {
            StartCoroutine(changeLine("left"));
            lineCount--;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow) && lineCount <= 0) {
            StartCoroutine(changeLine("right"));
            lineCount++;
        }
    }

    IEnumerator changeLine(string where) {
        if(where == "left") {
            rb.velocity = new Vector2(-5f, rb.velocity.y);
            Debug.Log("Left");

            yield return new WaitForSeconds(1f);

            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2(5f, rb.velocity.y);
            Debug.Log("Right");

            yield return new WaitForSeconds(1f);

            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
