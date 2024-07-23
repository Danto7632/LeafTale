using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameClear : MonoBehaviour {
    public RectTransform pos;

    public TMP_Text textScore;
    public string text;
    public bool clear;

    void Start() {
        textScore = GameObject.Find("GameScore").GetComponent<TMP_Text>();
        pos = GetComponent<RectTransform>();

        pos.anchoredPosition = new Vector2(0, 1000);

        clear = false;
    }

    void Update() {
        if(clear && Input.GetKeyDown("p")) SceneManager.LoadScene("StageSelect");
    }

    public void Clear(int score) {
        pos.anchoredPosition = new Vector2(0, 0);
        text = "your score is " + score.ToString();
        textScore.text = text;
        clear = true;
    }
}
