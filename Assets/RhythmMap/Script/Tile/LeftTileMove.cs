using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTileMove : MonoBehaviour { 

    public Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(BeforeGame.isGameStart) {
            rb.velocity = new Vector2(2f, 0);
        }
    }
}
