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
                Debug.Log("1�� ���������� �̵�");
                break;
            case "Level (2)":
                Debug.Log("2�� ���������� �̵�");
                break;
            case "Level (3)":
                Debug.Log("3�� ���������� �̵�");
                break;
            case "Level (4)":
                Debug.Log("4�� ���������� �̵�");
                break;
            case "Level (5)":
                Debug.Log("5�� ���������� �̵�");
                break;
            default:
                Debug.Log("��ư �̸��� �߸� �־����ϴ�.");
                break;
        }

    }
}
