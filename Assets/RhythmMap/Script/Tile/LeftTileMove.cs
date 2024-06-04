using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTileMove : MonoBehaviour {
    public Rigidbody2D rb;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.velocity = new Vector2(2f, rb.velocity.y);
    }
}
