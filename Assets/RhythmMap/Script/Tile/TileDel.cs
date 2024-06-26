using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDel : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("tileCheck")) {
            Destroy(gameObject, 1f);
            Debug.Log("Fail");
        }
    }
}
