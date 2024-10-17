using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {
    public Transform cam;

    public float relativeMove = 0.3f;
    public float posX = 0f;
    public float posY = 0f;

    void Awake() {
        cam = GameObject.FindWithTag("MainCamera").transform;

        if (gameObject.CompareTag("Background1")) {
            relativeMove = 0.84f;
            posX = 0f;
            posY = 3.5f;
        } 
        else if (gameObject.CompareTag("Background2")) {
            relativeMove = 0.9f;
            posX = 0f;
            posY = 2f;
        }
        else if (gameObject.CompareTag("Background3")) {
            relativeMove = 0.93f;
            posX = 0f;
            posY = 3f;
        }
        else if (gameObject.CompareTag("Background4")) {
            relativeMove = 0.95f;
            posX = 0f;
            posY = 0f;
        }
    }

    void LateUpdate() {
        if(cam != null) transform.position = new Vector2((cam.position.x - posX) * relativeMove, (cam.position.y - posY) * relativeMove);
    }
}