using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyMove : MonoBehaviour {
    public Rigidbody2D rb;
    public Animator anim;
    public broomMove broomStatus;

    public GameObject Player;

    public Vector2 direction;
    public float moveSpeed;
    
    void Awake() {
        Player = GameObject.FindWithTag("Player");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        broomStatus = Player.GetComponent<broomMove>();

        moveSpeed = 10f;
        direction = new Vector2(0, 0);
        Invoke("DirectToPlayer", 3f);
    }

    void Update() {
        rb.velocity = new Vector2(direction.x * moveSpeed, -moveSpeed);

        if(this.gameObject.transform.position.y <= -4f) {
            Destroy(this.gameObject);
        }

        if(broomStatus.isGameOver || broomStatus.isGameClear) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && !broomStatus.isHit) {
            Destroy(this.gameObject);
        }
    }

    void DirectToPlayer() {
        Vector2 toDirect;
        toDirect = (Player.transform.position - this.transform.position).normalized;
        direction = new Vector2(toDirect.x, direction.y);
    }
}
