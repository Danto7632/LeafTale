using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTileCheck : MonoBehaviour {
    public GameObject rockTile;
    public GameObject paperTile;
    public GameObject scissorTile;

    public bool isRock;
    public bool isPaper;
    public bool isScissor;

    public LayerMask tileLayer;

    public void Awake() {
        isRock = true;
        isPaper = false;
        isScissor = false;

        tileLayer = LayerMask.GetMask("RhythmTile");
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

        rockTile.SetActive(isRock);
        paperTile.SetActive(isPaper);
        scissorTile.SetActive(isScissor);

        RaycastHit2D checkRay = Physics2D.Raycast(new Vector2(0, 1), Vector2.down, 2f, tileLayer);

        if(checkRay.collider != null) {
            if(checkRay.collider.gameObject.CompareTag("RockTile") && isRock) {
                Destroy(checkRay.collider.gameObject);
                Debug.Log("RockCheck!");
            }
            else if(checkRay.collider.gameObject.CompareTag("PaperTile") && isPaper) {
                Destroy(checkRay.collider.gameObject);
                Debug.Log("PaperCheck!");
            }
            else if(checkRay.collider.gameObject.CompareTag("ScissorTile") && isScissor) {
                Destroy(checkRay.collider.gameObject);
                Debug.Log("ScissorCheck!");
            }
        }
    }
}