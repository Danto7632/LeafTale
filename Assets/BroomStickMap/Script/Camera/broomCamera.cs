using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class broomCamera : MonoBehaviour {
    public GameObject Player;
    public float xFixedPosition = 0f;
    public float smoothSpeed;

    void Awake() {
        smoothSpeed = 0.1f;
        Player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate() {
        if (Player != null) {
            Vector3 desiredPosition = new Vector3(xFixedPosition, Player.transform.position.y + 3f, transform.position.z);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }   
}