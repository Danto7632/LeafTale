using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBar : MonoBehaviour {

    public GameObject[] StartBarImage = new GameObject[4];

    void Update() {
        if(BeforeGame.elapsedTime <= 0.1f) {
            StartBarImage[0].gameObject.SetActive(true);
            StartBarImage[1].gameObject.SetActive(false);
            StartBarImage[2].gameObject.SetActive(false);
            StartBarImage[3].gameObject.SetActive(false);
        }
        else if(BeforeGame.elapsedTime <= 0.5f) {
            StartBarImage[0].gameObject.SetActive(false);
            StartBarImage[1].gameObject.SetActive(true);
            StartBarImage[2].gameObject.SetActive(false);
            StartBarImage[3].gameObject.SetActive(false);
        }
        else if(BeforeGame.elapsedTime <= 1.5f) {
            StartBarImage[0].gameObject.SetActive(false);
            StartBarImage[1].gameObject.SetActive(false);
            StartBarImage[2].gameObject.SetActive(true);
            StartBarImage[3].gameObject.SetActive(false);
        }
        else if(BeforeGame.elapsedTime <= 2.5f) {
            StartBarImage[0].gameObject.SetActive(false);
            StartBarImage[1].gameObject.SetActive(false);
            StartBarImage[2].gameObject.SetActive(false);
            StartBarImage[3].gameObject.SetActive(true);
        }
    }
}
