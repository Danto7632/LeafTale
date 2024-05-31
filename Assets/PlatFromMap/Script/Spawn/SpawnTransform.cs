using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTransform : MonoBehaviour {
    public SpawnPointManager spawnPoint;
    public bool isFirstTouch = false;

    void Start() {
        spawnPoint = transform.parent.GetComponent<SpawnPointManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!isFirstTouch && other.gameObject.CompareTag("Player")) {
            ++spawnPoint.spawnCount;
            isFirstTouch = true;
        }
    }
}
