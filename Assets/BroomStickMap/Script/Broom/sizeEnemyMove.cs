using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sizeEnemyMove : MonoBehaviour {
    public Rigidbody2D rb;
    public Animator anim;
    public broomMove broomStatus;

    public Vector2 originSize;
    public float moveSpeed;
    public bool isDown;
    
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        broomStatus = GameObject.FindWithTag("Player").GetComponent<broomMove>();

        isDown = false;
        moveSpeed = 8f;
        originSize = transform.localScale;

        InvokeRepeating("sizeChange", 0, 0.1f);
    }

    void Update() {
        rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
        
        if(this.gameObject.transform.position.y <= -4f) {
            Destroy(this.gameObject);
        }

        if(broomStatus.isGameOver || broomStatus.isGameClear) {
            Destroy(this.gameObject);
        }
    }

    void sizeChange() {
        if(transform.localScale.x <= (originSize.x * 2f) && !isDown) {
            transform.localScale = new Vector2(transform.localScale.x + 000.1f, transform.localScale.y + 000.1f);
        }
        
        if(transform.localScale.x > (originSize.x * 2f) || isDown) {
            isDown = true;
            transform.localScale = new Vector2(transform.localScale.x - 000.1f, transform.localScale.y - 000.1f);
        }

        if(transform.localScale.x <= originSize.x) {
            isDown = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && !broomStatus.isHit) {
            Destroy(this.gameObject);
        }
    }
}
