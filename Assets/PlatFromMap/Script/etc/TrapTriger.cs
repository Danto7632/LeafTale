using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTriger : MonoBehaviour {
    public catMove cat;

    public int spawnCount;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            cat = other.gameObject.GetComponent<catMove>();
            spawnCount = GameObject.Find("SpawnPoint").GetComponent<SpawnPointManager>().spawnCount;

            switch (spawnCount) {
                case 0 :
                    if (!cat.isFacingRight) {
                        cat.newScale = cat.transform.localScale;
                        cat.newScale.x *= -1f;

                        cat.transform.localScale = cat.newScale;
                        cat.isFacingRight = true;
                    }
                    other.transform.position = new Vector2(-18f, 2.8f);
                    break;

                case 1 :
                    if (!cat.isFacingRight) {
                        cat.newScale = cat.transform.localScale;
                        cat.newScale.x *= -1f;

                        cat.transform.localScale = cat.newScale;
                        cat.isFacingRight = true;
                    }
                    other.transform.position = new Vector2(28f, 7f);
                    break;

                case 2 :
                    if (cat.isFacingRight) {
                        cat.newScale = cat.transform.localScale;
                        cat.newScale.x *= -1f;

                        cat.transform.localScale = cat.newScale;
                        cat.isFacingRight = false;
                    }
                    other.transform.position = new Vector2(59f, 11f);
                    break;

                case 3 :
                    if (cat.isFacingRight) {
                        cat.newScale = cat.transform.localScale;
                        cat.newScale.x *= -1f;

                        cat.transform.localScale = cat.newScale;
                        cat.isFacingRight = false;
                    }
                    other.transform.position = new Vector2(26f, 17f);
                    break;
            }
        }
    }
}
