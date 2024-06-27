using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    public Rigidbody2D rb;
    public Animator anim;

    public float moveSpeed;

    public GameObject Player;
    public broomMove broomStatus;
    
    void Awake() {
        Player = GameObject.FindWithTag("Player");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        broomStatus = Player.GetComponent<broomMove>();

        moveSpeed = 8f;
    }

    void Update() {
        rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);

        if(this.gameObject.transform.position.y <= -4) {
            Destroy(this.gameObject);
        }

        if(broomStatus.isGameOver) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && !broomStatus.isHit) {
            Destroy(this.gameObject);
        }
    }
}
