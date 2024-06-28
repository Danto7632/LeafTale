using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoStage : MonoBehaviour
{
    public GameObject tempBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel()
    {
        tempBtn = EventSystem.current.currentSelectedGameObject;
        switch (tempBtn.name)
        {
            case "Level (1)":
                Debug.Log("1번 스테이지로 이동");
                break;
            case "Level (2)":
                Debug.Log("2번 스테이지로 이동");
                break;
            case "Level (3)":
                Debug.Log("3번 스테이지로 이동");
                break;
            case "Level (4)":
                Debug.Log("4번 스테이지로 이동");
                break;
            case "Level (5)":
                Debug.Log("5번 스테이지로 이동");
                break;
            default:
                Debug.Log("버튼 이름을 잘못 주었습니다.");
                break;
        }

    }
}
