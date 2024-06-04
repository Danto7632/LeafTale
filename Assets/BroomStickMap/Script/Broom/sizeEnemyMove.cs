using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sizeEnemyMove : MonoBehaviour {
    public Rigidbody2D rb;
    public Animator anim;

    public float moveSpeed;
    
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        moveSpeed = 10f;

        StartCoroutine(sizeChange());
    }

    void Update() {
        rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);

        if(this.gameObject.transform.position.y <= -10f) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            Destroy(this.gameObject);
        }
    }

    IEnumerator sizeChange() {
        for(int i = 0; i < 10; i++) {
            transform.localScale = new Vector2(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f);

            yield return new WaitForSeconds(0.2f);
        }

        for(int i = 0; i < 10; i++) {
            transform.localScale = new Vector2(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f);

            yield return new WaitForSeconds(0.2f);
        }

        StartCoroutine(sizeChange());
    }
}
