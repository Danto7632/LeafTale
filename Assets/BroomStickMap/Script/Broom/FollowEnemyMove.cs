using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyMove : MonoBehaviour {
    public GameObject player;

    public Rigidbody2D rb;
    public Animator anim;

    public Vector2 direction;

    public float moveSpeed;
    
    void Awake() {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        moveSpeed = 5f;
        InvokeRepeating("DirectToPlayer", 0, 1f);
    }

    void Update() {
        rb.velocity = new Vector2(direction.x * moveSpeed, -moveSpeed);

        if(this.gameObject.transform.position.y <= -10) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            Destroy(this.gameObject);
        }
    }

    void DirectToPlayer() {
        Vector2 toDirect;
        toDirect = (player.transform.position - this.transform.position).normalized;
        direction = new Vector2(toDirect.x, direction.y);
    }
}