using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // SceneManager
using TMPro; // TMP_Text
using UnityEngine.Networking; // UnityWebRequest
using System; // DateTime
using Leap;
using Leap.Unity;

public class GameClear : MonoBehaviour
{

    private LeapServiceProvider leapProvider;

    public RectTransform pos;
    public TMP_Text textScore;
    public string text;
    public bool clear;
    public bool isNext;
    private string postUrl;
    private string postUrl2;

    public AudioSource chargeSound;

    public UnityEngine.UI.Image gaugeImage;

    private void Start()
    {
        textScore = GameObject.Find("GameScore").GetComponent<TMP_Text>();
        pos = GetComponent<RectTransform>();
        pos.anchoredPosition = new Vector2(0, 10000);
        clear = false;
        postUrl = "http://43.203.0.69:8080/api/Pmdata/score";
        postUrl2 = "http://43.203.0.69:8080/api/games/singleGame";

        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        ResetGauge();
    }

    private void Update()
    {
        if (clear && Input.GetKeyDown(KeyCode.M))
        {
            if (StoryOrStage.instance == null)
            {
                changeScene_B("StageSelect");
            }
            else if (StoryOrStage.instance.currentMode == "stage")
            {
                changeScene_B("StageSelect");
            }
            else if (StoryOrStage.instance.currentMode == "story")
            {
                storyOrder();
            }
        }
    }

    public bool isPointing;
    public float pointingStartTime = 0f;
    public float elapsedTime = 0f;

    void OnUpdateFrame(Frame frame) {
        if(clear) {
            if (frame.Hands.Count > 0) { //사용자가 손을 인식하고 있는지
                Hand hand = frame.Hands[0]; // 인식한 손 중 맨 처음에 인식한 손 하나를 hand변수에 참조

                if (IsPointingPose(hand)) { //인식한 손이 가르키는 손동작을 하고 있는지 확인
                    if (!isPointing) {
                        pointingStartTime = Time.time;

                        isPointing = true;
                        chargeSound.Play();
                    } //특정 손동작을 인식한 시간을 저장
                    else {
                        elapsedTime = Time.time - pointingStartTime;

                        UpdateGauge(elapsedTime);
                        if (elapsedTime > 3f) { //특정 손동작이 3초 이상 지속되는지 확인 후 게임 실행
                           if (StoryOrStage.instance == null) {
                                changeScene_B("StageSelect");
                            }
                            else if (StoryOrStage.instance.currentMode == "stage") {
                                changeScene_B("StageSelect");
                            }
                            else if (StoryOrStage.instance.currentMode == "story") {
                                storyOrder();
                            }

                            chargeSound.Stop();
                        }

                    }
                }
                else {
                    ResetGauge();
                    elapsedTime = 0f;
                    isPointing = false;
                    StartBar.ChangeHealthBarAmount(elapsedTime);
                }
            }
        }
    }

    void UpdateGauge(float time) {
        if (gaugeImage != null) {
            gaugeImage.fillAmount = Mathf.Clamp01(time / 3f);
        }
    }

    void ResetGauge() {
        if (gaugeImage != null) {
            gaugeImage.fillAmount = 0f;
        }
    }

    public void Clear(int score)
    {
        isNext = true;
        pos.anchoredPosition = new Vector2(0, 0);
        text = "your score is " + score.ToString();
        textScore.text = text;
        clear = true;

        string gameId = ""; // gameId를 저장할 변수
        if (SceneManager.GetActiveScene().name == "platformScene")
        {
            StoryOrStage.instance.G001Cleared = true;
            StoryOrStage.instance.G001Score = score;
            gameId = "G001"; // platformScene -> G001
            Debug.Log("G001 Cleared");
        }
        else if (SceneManager.GetActiveScene().name == "BroomstickScene")
        {
            StoryOrStage.instance.G002Cleared = true;
            StoryOrStage.instance.G002Score = score;
            gameId = "G002"; // BroomstickScene -> G002
            Debug.Log("G002 Cleared");
        }
        else if (SceneManager.GetActiveScene().name == "RhythmScene")
        {
            StoryOrStage.instance.G003Cleared = true;
            StoryOrStage.instance.G003Score = score;
            gameId = "G003"; // RhythmScene -> G003
            Debug.Log("G003 Cleared");
        }
        else if (SceneManager.GetActiveScene().name == "test")
        {
            StoryOrStage.instance.G004Cleared = true;
            StoryOrStage.instance.G004Score = score;
            gameId = "G004"; // test -> G004
            Debug.Log("G004 Cleared");
        }
        else if (SceneManager.GetActiveScene().name == "ClawMachineScenes")
        {
            StoryOrStage.instance.G005Cleared = true;
            StoryOrStage.instance.G005Score = score;
            gameId = "G005"; // ClawMachineScenes -> G005
            Debug.Log("G005 Cleared");
        }

        LogGameClearStatus();

        string playerId = Login.LoadEncryptedData("userID");

        // 플래그에 따라 데이터 전송 형식 결정
        if (StoryOrStage.instance.currentMode == "stage")
        {
            // stage 모드일 때는 현재 클리어한 게임만 전송
            StartCoroutine(SaveStageScore(gameId, score));
        }
        else if (StoryOrStage.instance.currentMode == "story" && StoryOrStage.instance.AllGamesCleared())
        {
            // story 모드일 때는 모든 게임이 클리어되었을 때만 전송
            StartCoroutine(SaveAllScores());
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

    IEnumerator SaveStageScore(string gId, int gScore)
    {
        var scoreData = new StagePostData
        {
            memberId = Login.LoadEncryptedData("userID"),
            gameId = gId,
            gameScore = gScore
        };

        string jsonData = JsonUtility.ToJson(scoreData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest www = new UnityWebRequest(postUrl2, UnityWebRequest.kHttpVerbPOST);
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
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "ClawMachineScenes")
        {
            StoryOrStage.instance.nextStory = "ClawMachineScenes";
            changeScene_B("StoryPage");
        }
        else if (sceneName == "BroomstickScene")
        {
            StoryOrStage.instance.nextStory = "BroomstickScene";
            changeScene_B("StoryPage");
        }
        else if (sceneName == "platformScene")
        {
            StoryOrStage.instance.nextStory = "platformScene";
            changeScene_B("StoryPage");
        }
        else if (sceneName == "RhythmScene")
        {
            StoryOrStage.instance.nextStory = "RhythmScene";
            changeScene_B("StoryPage");
        }
        else if (sceneName == "test")
        {
            StoryOrStage.instance.nextStory = "test";
            changeScene_B("StoryPage");
        }
    }

    //scene change
    void changeScene_B(string sceneName)
    {
        GameObject.Find("LoadScene_B").GetComponent<DemoLoadScene>().LoadScene(sceneName);
    }


    bool IsPointingPose(Hand hand) {
        foreach (Finger finger in hand.Fingers) { //손의 손가락을 모두 가져와 반복문 실행
            if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                if (!finger.IsExtended) return false;
            } //검지가 펴져있지 않다면 false를 반환
            else {
                if (finger.IsExtended) return false;
            } //검지를 제외한 다른 손가락이 펴져있다면 false를 반환
        }
        
        return true; //검지만 펴져있다면 true를 반환
    }
}

[System.Serializable]
public class StagePostData
{
    public string memberId;
    public string gameId;
    public int gameScore;
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
