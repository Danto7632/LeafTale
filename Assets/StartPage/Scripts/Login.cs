using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    public GameObject rank;
    public GameObject pressStart;

    [Header("Login")]
    public GameObject loginPanel;
    public Button loginButton;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pressStart.activeSelf == true && Input.GetMouseButtonDown(0))
            SceneManager.LoadScene("StoryPage");
    }
    public void LoginBtn()
    {
        loginPanel.SetActive(false);
        rank.SetActive(true);
        pressStart.SetActive(true);
    }
}
