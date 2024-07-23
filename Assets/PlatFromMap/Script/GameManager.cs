using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public TMP_Text scoreText;

    public int score = 0;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        

        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
    }

    public void AddScore(int newScore) {
        score += newScore;
        scoreText.text = "점수 : " + score; 
    }

    public void EndGame(int time, int maxTime) {
        double finScore;
        score += time;
        finScore = (score / (100.0 + maxTime))*100.0;
        GameObject.Find("GameClear").GetComponent<GameClear>().Clear((int)finScore);
    }
}
