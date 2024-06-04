using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDel : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("tileCheck")) {
            Invoke("DeleteTile", 0.6f);
        }
    }

    void DeleteTile() {
        Destroy(gameObject);
    }
}
