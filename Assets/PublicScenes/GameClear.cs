using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameClear : MonoBehaviour {
    public RectTransform pos;

    public TMP_Text textScore;
    public string text;
    public bool clear;

    private string url;
    private Scene scene;
    private int current_gameid;

    [System.Serializable]
    public class ScoreData // 변경될 수도
    {
        public int gameId;
        public string playerId;
        public int gameScore;
        public DateTime gameDate;
    }

    void Start() {
        textScore = GameObject.Find("GameScore").GetComponent<TMP_Text>();
        pos = GetComponent<RectTransform>();

        pos.anchoredPosition = new Vector2(0, 1000);

        clear = false;

        url = "http://43.203.0.69:8080/api/gamePMData";

        scene = SceneManager.GetActiveScene();
    }

    void Update() {
        if(clear && Input.GetKeyDown("p")) SceneManager.LoadScene("StageSelect");
    }

    public void Clear(int score) {
        pos.anchoredPosition = new Vector2(0, 0);
        text = "your score is " + score.ToString();
        textScore.text = text;
        clear = true;

        if (scene.name == "platformScene")
            current_gameid = 1; // 자료형이나 대입값이 바뀔 수 있음
        else if (scene.name == "BroomstickScene")
            current_gameid = 2; // 자료형이나 대입값이 바뀔 수 있음
        else if (scene.name == "RhythmScene")
            current_gameid = 3; // 자료형이나 대입값이 바뀔 수 있음

        //StartCoroutine(ScoreSave(current_gameid, score));
    }

    IEnumerator ScoreSave(int cgameId, int score) // gameId 자료형 바뀔 수도
    {
        var scoreData = new ScoreData
        {
            gameId = cgameId,
            playerId = Login.LoadEncryptedData("userID"),
            gameScore = score,
            gameDate = DateTime.Now
        };
        string jsonData = JsonUtility.ToJson(scoreData);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            print(www.downloadHandler.text);
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Request Error: " + www.error);
        }
        else
        {
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Response: " + www.downloadHandler.text);
            if (www.responseCode == 200 && www.downloadHandler.text == "true") // 수정/제거할 수도
            {
               
            }
            else if (www.responseCode == 200 && www.downloadHandler.text != "true") // 수정/제거할 수도
            {
                
            }
        }
    }
    
}
