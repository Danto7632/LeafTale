using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GoStage : MonoBehaviour {
    public static void SelectLevel() {
        switch (ScrollStage.btnIndex) {
            case 0 :
                SceneManager.LoadScene("BroomstickScene");
                break;

            case 1 :
                SceneManager.LoadScene("platformScene");
                break;

            case 2 :
                SceneManager.LoadScene("RhythmScene");
                break;

            case 3 :
                break;

            case 4 :
                break;

            default :
                break;
        }
    }
}
