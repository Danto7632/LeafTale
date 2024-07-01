using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // Add this line to access SceneManager

public class GoStage : MonoBehaviour
{
    public GameObject tempBtn;

    public void SelectLevel()
    {
        tempBtn = EventSystem.current.currentSelectedGameObject;
        switch (tempBtn.name)
        {
            case "Level (1)":
                SceneManager.LoadScene("BroomstickScene");
                break;
            case "Level (2)":
                SceneManager.LoadScene("platformScene");
                break;
            case "Level (3)":
                SceneManager.LoadScene("RhythmScene");
                break;
            case "Level (4)":
                // Add more cases as needed
                break;
            case "Level (5)":
                // Add more cases as needed
                break;
            default:
                Debug.Log("해당하는 씬이 없습니다.");
                break;
        }
    }
}
