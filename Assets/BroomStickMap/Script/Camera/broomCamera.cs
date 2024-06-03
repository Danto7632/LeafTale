using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class broomCamera : MonoBehaviour {
    public GameObject Player;
    public float xFixedPosition = 0f;
    public float smoothSpeed;

    [Header("Movement_Limits")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Awake() {
        smoothSpeed = 0.1f;
        Player = GameObject.FindWithTag("Player");

        minX = -10f;
        maxX = 10f;
        minY = -2f;
        maxY = 15f;
    }

    void FixedUpdate() {
        if (Player != null) {
            Vector3 desiredPosition = new Vector3(xFixedPosition, Player.transform.position.y + 3f, transform.position.z);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;

            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);

            transform.position = smoothedPosition;
        }
    }   
}