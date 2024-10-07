using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager
using TMPro; // TMP_Text
using UnityEngine.Networking; // UnityWebRequest
using System; // DateTime

public class GameClear : MonoBehaviour
{
    public RectTransform pos;
    public TMP_Text textScore;
    public string text;
    public bool clear;
    public bool isPointing;
    public bool isNext; // isNext 변수를 추가

    private string postUrl;

    private void Start()
    {
        textScore = GameObject.Find("GameScore").GetComponent<TMP_Text>();
        pos = GetComponent<RectTransform>();

        pos.anchoredPosition = new Vector2(0, 10000);
        clear = false;

        postUrl = "http://43.203.0.69:8080/api/Pmdata/score"; // Post URL 설정
    }

    private void Update()
    {
        // P 키를 눌렀을 때 씬 전환
        if (clear && Input.GetKeyDown(KeyCode.P))
        {
            if (StoryOrStage.instance == null)
            {
                SceneManager.LoadScene("StageSelect");
            }
            else if (StoryOrStage.instance.currentMode == "stage")
            {
                SceneManager.LoadScene("StageSelect");
            }
            else if (StoryOrStage.instance.currentMode == "story")
            {
                storyOrder();
            }
        }
    }

    public void Clear(int score)
    {
        isNext = true; // 게임 클리어 시 isNext를 true로 설정
        pos.anchoredPosition = new Vector2(0, 0);
        text = "your score is " + score.ToString();
        textScore.text = text;
        clear = true;

        // 게임 종류에 따라 gameId 지정 및 점수, 플래그 저장
        if (SceneManager.GetActiveScene().name == "platformScene")
        {
            StoryOrStage.instance.G001Cleared = true;
            StoryOrStage.instance.G001Score = score;
            Debug.Log("G001 Cleared");
        }
        else if (SceneManager.GetActiveScene().name == "BroomstickScene")
        {
            StoryOrStage.instance.G002Cleared = true;
            StoryOrStage.instance.G002Score = score;
            Debug.Log("G002 Cleared");
        }
        else if (SceneManager.GetActiveScene().name == "RhythmScene")
        {
            StoryOrStage.instance.G003Cleared = true;
            StoryOrStage.instance.G003Score = score;
            Debug.Log("G003 Cleared");
        }
        else if (SceneManager.GetActiveScene().name == "test")
        {
            StoryOrStage.instance.G004Cleared = true;
            StoryOrStage.instance.G004Score = score;
            Debug.Log("G004 Cleared");
        }
        else if (SceneManager.GetActiveScene().name == "ClawMachineScenes")
        {
            StoryOrStage.instance.G005Cleared = true;
            StoryOrStage.instance.G005Score = score;
            Debug.Log("G005 Cleared");
        }

        // 모든 게임 클리어 상태 로그 출력
        LogGameClearStatus();

        string playerId = Login.LoadEncryptedData("userID");

        // 모든 게임이 클리어되었을 때만 점수 전송
        if (StoryOrStage.instance.AllGamesCleared())
        {
            StartCoroutine(SaveAllScores());
            // 점수 전송 후에 모든 점수 초기화 및 플래그 초기화 고민중
        }
    }

    void LogGameClearStatus()
    {
        Debug.Log($"Game Clear Status: \n" +
                  $"G001 Cleared: {StoryOrStage.instance.G001Cleared}, Score: {StoryOrStage.instance.G001Score}\n" +
                  $"G002 Cleared: {StoryOrStage.instance.G002Cleared}, Score: {StoryOrStage.instance.G002Score}\n" +
                  $"G003 Cleared: {StoryOrStage.instance.G003Cleared}, Score: {StoryOrStage.instance.G003Score}\n" +
                  $"G004 Cleared: {StoryOrStage.instance.G004Cleared}, Score: {StoryOrStage.instance.G004Score}\n" +
                  $"G005 Cleared: {StoryOrStage.instance.G005Cleared}, Score: {StoryOrStage.instance.G005Score}");
    }

    IEnumerator SaveAllScores()
    {
        yield return ScoreSave(StoryOrStage.instance.G001Score, StoryOrStage.instance.G002Score, StoryOrStage.instance.G003Score, StoryOrStage.instance.G004Score, StoryOrStage.instance.G005Score);
        Debug.Log("All scores posted successfully.");
    }

    IEnumerator ScoreSave(int score1, int score2, int score3, int score4, int score5)
    {
        var scoreData = new PostData
        {
            memberId = Login.LoadEncryptedData("userID"),
            gameScore1 = score1,
            gameScore2 = score2,
            gameScore3 = score3,
            gameScore4 = score4,
            gameScore5 = score5,
        };

        string jsonData = JsonUtility.ToJson(scoreData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest www = new UnityWebRequest(postUrl, UnityWebRequest.kHttpVerbPOST);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log("Score post complete! Response: " + www.downloadHandler.text);

            Debug.Log("Response Code: " + www.responseCode);
        }
    }

    void storyOrder()
    {
        // 스토리 순서에 따른 씬 전환 로직
        string sceneName = SceneManager.GetActiveScene().name;

        Debug.Log(sceneName + "의 클리어 후 스토리...");
        if (sceneName == "BroomstickScene")
        {
            SceneManager.LoadScene("platformScene");
        }
        else if (sceneName == "platformScene")
        {
            SceneManager.LoadScene("RhythmScene");
        }
        else if (sceneName == "RhythmScene")
        {
            SceneManager.LoadScene("test");
        }
        else if (sceneName == "test")
        {
            SceneManager.LoadScene("ClawMachineScenes");
        }
        else if (sceneName == "ClawMachineScenes")
        {
            Debug.Log("모두 클리어");
            SceneManager.LoadScene("EndGame");
        }
    }

    /*
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
    */
}

[System.Serializable]
public class PostData
{
    public string memberId;
    public int gameScore1;
    public int gameScore2;
    public int gameScore3;
    public int gameScore4;
    public int gameScore5;
}
