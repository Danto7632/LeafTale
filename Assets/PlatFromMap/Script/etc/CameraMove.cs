using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cameramove : MonoBehaviour {
    public Transform target;

    public float smoothSpeed = 2;
    public Vector2 offset;
    float cameraHalfWidth, cameraHalfHeight;

    private void Awake() {
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Start() {
        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        cameraHalfHeight = Camera.main.orthographicSize;
    }
}