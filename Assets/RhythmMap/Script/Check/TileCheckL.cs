using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCheckL : MonoBehaviour {
    public bool isRock;
    public bool isPaper;
    public bool isScissor;

    public GameObject rockPrefab;
    public GameObject paperPrefab;
    public GameObject scissorPrefab;

    public RaycastHit2D[] isTileCheck = new RaycastHit2D[3];
    public Vector2[] tileRayVec = new Vector2[3];

    public string layerName;

    public GameObject score;

    public void Awake() {
        isRock = false;
        isPaper = false;
        isScissor = false;

        tileRayVec[0] = new Vector2(-1.5f, 1f);
        tileRayVec[1] = new Vector2(-1.0f, 1f);
        tileRayVec[2] = new Vector2(-0.2f, 1f);

        score = GameObject.Find("GameManager");
    }

    public void getLeap(string hands) {
        if(BeforeGame.isGameStart) {
            if(hands == "ROCK") {
                isRock = true;
                isPaper = false;
                isScissor = false;
                layerName = "RockTile";
            }
            else if(hands == "SCISSOR") {
                isRock = false;
                isPaper = false;
                isScissor = true;
                layerName = "ScissorsTile";
            }
            else if(hands == "PAPER") {
                isRock = false;
                isPaper = true;
                isScissor = false;
                layerName = "PaperTile";
            }

            rockPrefab.SetActive(isRock);
            paperPrefab.SetActive(isPaper);
            scissorPrefab.SetActive(isScissor);
        }
    }

    void Update() {
        for(int i = 0; i < 3; i++) {
            Vector2 origin = tileRayVec[i];
            Vector2 direction = Vector2.down;
            float distance = 1f;

            Debug.DrawRay(origin, direction * distance, Color.red);

            isTileCheck[i] = Physics2D.Raycast(origin, direction, distance);
            

            if(isTileCheck[i].collider != null && isTileCheck[i].collider.gameObject.layer == LayerMask.NameToLayer(layerName)) {
                switch(i) {
                    case 0 :
                        Debug.Log("Great");
                        score.GetComponent<RhythmScore>().NodeHit(2);
                        Destroy(isTileCheck[i].collider.gameObject);
                        break;
                    case 1 :
                        Debug.Log("Soso");
                        score.GetComponent<RhythmScore>().NodeHit(1);
                        Destroy(isTileCheck[i].collider.gameObject);
                        break;
                }
            }
            else if(isTileCheck[i].collider != null && isTileCheck[i].collider.gameObject.layer != LayerMask.NameToLayer(layerName) && i == 2) {
                score.GetComponent<RhythmScore>().NodeHit(0);
                Debug.Log("Fail");
                Destroy(isTileCheck[i].collider.gameObject);
            }
        }
    }
}
