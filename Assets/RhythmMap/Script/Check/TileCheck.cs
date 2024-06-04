using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCheck : MonoBehaviour {
    public bool isRock;
    public bool isPaper;
    public bool isScissor;

    public void Awake() {
        isRock = true;
        isPaper = false;
        isScissor = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("RockTile")) {
            if(isRock) {
                Debug.Log("Clear!");
            }
            else {
                Debug.Log("Fail!");
            }
        }

        if(other.gameObject.CompareTag("PaperTile")) {
            if(isPaper) {
                Debug.Log("Clear!");
            }
            else {
                Debug.Log("Fail!");
            }
        }

        if(other.gameObject.CompareTag("ScissorTile")) {
            if(isScissor) {
                Debug.Log("Clear!");
            }
            else {
                Debug.Log("Fail!");
            }
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            isRock = true;
            isPaper = false;
            isScissor = false;
        }
        else if(Input.GetKeyDown(KeyCode.W)) {
            isRock = false;
            isPaper = true;
            isScissor = false;
        }
        else if(Input.GetKeyDown(KeyCode.E)) {
            isRock = false;
            isPaper = false;
            isScissor = true;
        }
    }
}
