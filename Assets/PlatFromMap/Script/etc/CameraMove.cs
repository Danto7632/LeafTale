using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramove : MonoBehaviour {
    public Transform target;

    public Vector2 offset;
    public float smoothSpeed = 2;
    public float limitMinX, limitMaxX, limitMinY, limitMaxY;
    public float cameraHalfWidth, cameraHalfHeight;

    void Awake() {
        target = GameObject.FindWithTag("Player").transform;

        limitMaxX = 73.4f;
        limitMinX = -16.5f;
        limitMaxY = 50.4f;
        limitMinY = -4.8f;
    }

    void Start() {
        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        cameraHalfHeight = Camera.main.orthographicSize;
    }

    private void FixedUpdate() {
        Vector3 desiredPosition = new Vector3 (
            Mathf.Clamp(target.position.x + offset.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),
            Mathf.Clamp(target.position.y + offset.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), 
            -10
        );
                                                                                                // Z
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}