using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class GameClear : MonoBehaviour {
    public LeapServiceProvider leapProvider;
    public Hand hand;

    public RectTransform pos;

    public TMP_Text textScore;
    public string text;
    public bool clear;
    public bool isPointing;

    private string url;
    private Scene scene;
    private int current_gameid;

    public float elapsedTime;
    public float pointingStartTime;

    [System.Serializable]
    public class ScoreData
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

        url = "http://43.203.0.69:8080/api/Pmdata/score";

        scene = SceneManager.GetActiveScene();

        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        isPointing = false;
        elapsedTime = 0f;
    }

    void Update() {
        if(clear && Input.GetKeyDown("p")) SceneManager.LoadScene("StageSelect");
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            hand = frame.Hands[0];
            if(IsPointingPose(hand)) {
                if (!isPointing) {
                    pointingStartTime = Time.time;

                    isPointing = true;
                }
                else {
                    elapsedTime = Time.time - pointingStartTime;

                    if (elapsedTime > 3f && clear) {
                        SceneManager.LoadScene("StageSelect");
                    }
                }
            }
        }
    }

    public void Clear(int score) {
        pos.anchoredPosition = new Vector2(0, 0);
        text = "your score is " + score.ToString();
        textScore.text = text;
        clear = true;

        if (scene.name == "platformScene")
            current_gameid = 1; // �ڷ����̳� ���԰��� �ٲ� �� ����
        else if (scene.name == "BroomstickScene")
            current_gameid = 2; // �ڷ����̳� ���԰��� �ٲ� �� ����
        else if (scene.name == "RhythmScene")
            current_gameid = 3; // �ڷ����̳� ���԰��� �ٲ� �� ����

        //StartCoroutine(ScoreSave(current_gameid, score));

        string savedDateStr = PlayerPrefs.GetString(dateKey, "");
        int highScore = PlayerPrefs.GetInt(scoreKey, 0);

        DateTime savedDate;
        if (DateTime.TryParse(savedDateStr, out savedDate) && savedDate.Date == DateTime.Now.Date) {
            if (score > highScore) {
                PlayerPrefs.SetInt(scoreKey, score);
                PlayerPrefs.SetString(dateKey, DateTime.Now.ToString());
                StartCoroutine(ScoreSave(current_gameid, score));
            }
            else {
                PlayerPrefs.SetInt(scoreKey, score);
                PlayerPrefs.SetString(dateKey, DateTime.Now.ToString());
                StartCoroutine(ScoreSave(current_gameid, score));
            }
        }
    }

    IEnumerator ScoreSave(int cgameId, int score) // gameId �ڷ��� �ٲ� ����
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
            if (www.responseCode == 200 && www.downloadHandler.text == "true") // ����/������ ����
            {
               
            }
            else if (www.responseCode == 200 && www.downloadHandler.text != "true") // ����/������ ����
            {
                
            }
        }
    }

    bool IsPointingPose(Hand hand) {
        foreach (Finger finger in hand.Fingers) {
            if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                if (!finger.IsExtended) return false;
            }
            else {
                if (finger.IsExtended) return false;
            }
        }
        
        return true;
    }
}
