using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro; /// ui text를 불러오기위한 부분
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public TMP_Text textScore;
    string text;
    bool clear = false;
    RectTransform pos;
    // Start is called before the first frame update
    void Start()
    {
        textScore = GameObject.Find("GameScore").GetComponent<TMP_Text>();
        pos = GetComponent<RectTransform>();
        pos.anchoredPosition = new Vector2(0, 1000);
    }
    // Update is called once per framed
    void Update()
    {
        if(clear && Input.GetKeyDown("p"))
        {
            SceneManager.LoadScene("StageSelect");
        }
    }

    public void Clear(int score)
    {
        pos.anchoredPosition = new Vector2(0, 0);
        text = "your score is " + score.ToString();
        textScore.text = text;
        clear = true;
    }
}
