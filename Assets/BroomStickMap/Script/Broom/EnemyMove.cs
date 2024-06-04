using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    public Rigidbody2D rb;
    public Animator anim;

    public float moveSpeed;
    
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        moveSpeed = 8f;
    }

    void Update() {
        rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);

        if(this.gameObject.transform.position.y <= -4) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            Destroy(this.gameObject);
        }
    }
}
