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
        if(other.gameObject.CompareTag("Player")) {
            cat = other.gameObject.GetComponent<catMove>();

            switch(spawnCount) {
                case 0 :
                    other.transform.position = new Vector2(-18f, 2.8f);
                    break;
                case 1 :
                    other.transform.position = new Vector2(40f, 8.8f);
                    break;
                case 2 :
                    other.transform.position = new Vector2(59f, 11f);
                    break;
            }
        }
    }
}
