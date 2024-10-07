using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour {
    public catMove cat;

    public int spawnCount;

    public void Start() {
        spawnCount = 0;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            cat = other.gameObject.GetComponent<catMove>();

            switch (spawnCount) {
                case 0 :
                    if (!cat.isFacingRight) {
                        cat.newScale = cat.transform.localScale;
                        cat.newScale.x *= -1f;

                        cat.transform.localScale = cat.newScale;
                        cat.isFacingRight = true;
                    }
                    other.transform.position = new Vector2(-15f, 4f);
                    break;

                case 1 :
                    if (cat.isFacingRight) {
                        cat.newScale = cat.transform.localScale;
                        cat.newScale.x *= -1f;

                        cat.transform.localScale = cat.newScale;
                        cat.isFacingRight = false;
                    }
                    other.transform.position = new Vector2(65f, 11f);
                    break;

                case 2 :
                    if (!cat.isFacingRight) {
                        cat.newScale = cat.transform.localScale;
                        cat.newScale.x *= -1f;

                        cat.transform.localScale = cat.newScale;
                        cat.isFacingRight = true;
                    }
                    other.transform.position = new Vector2(-13f, 16f);
                    break;

                case 3 :
                    if (cat.isFacingRight) {
                        cat.newScale = cat.transform.localScale;
                        cat.newScale.x *= -1f;

                        cat.transform.localScale = cat.newScale;
                        cat.isFacingRight = false;
                    }
                    other.transform.position = new Vector2(67f, 23f);
                    break;
            }
        }
    }
}