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

    private string postUrl;
    private string getUrl;
    private Scene scene;
    private string currentGameId;
    private int beforeScore = -1;
    private int transScore;

    public float elapsedTime;
    public float pointingStartTime;

    [System.Serializable]
    public class GetData
    {
        public string gameId;
        public int gameScore;
        public string gameDate;
    }

    [System.Serializable]
    public class GetDataArray
    {
        public GetData[] scores;
    }

    [System.Serializable]
    public class PostData
    {
        public string gameId;
        public string memberId;
        public int gameScore;
        public string gameDate;

        // `gameDate`를 DateTime으로 변환하는 메서드
        public DateTime GetGameDate()
        {
            return DateTime.Parse(gameDate);
        }

        // `gameDate`를 설정하는 메서드
        public void SetGameDate(DateTime date)
        {
            gameDate = date.ToString("yyyy-MM-dd");
        }
    }

    void Start() {
        textScore = GameObject.Find("GameScore").GetComponent<TMP_Text>();
        pos = GetComponent<RectTransform>();

        pos.anchoredPosition = new Vector2(0, 1000);

        clear = false;

        postUrl = "http://43.203.0.69:8080/api/Pmdata/score";
        getUrl = "http://43.203.0.69:8080/api/games/member/{0}/game/{1}";

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

        // 게임 종류에 따라 gameId 지정
        if (scene.name == "platformScene")
            currentGameId = "G001";
        else if (scene.name == "BroomstickScene")
            currentGameId = "G002";
        else if (scene.name == "RhythmScene")
            currentGameId = "G003";

        // 암호화해서 로컬 저장한 memberId 불러옴
        string playerId = Login.LoadEncryptedData("userID");

        StartCoroutine(ProcessScore(playerId, currentGameId, score));

    }

    // api 통신을 통해 저장된 해당 게임의 점수를 가져오고 방금 클리어한 게임 스코어와 비교해
    // 현재 스코어가 더 크면 api post 통신으로 score 저장
    IEnumerator ProcessScore(string cplayerId, string pgameId, int pscore)
    {
        // api 통신을 통해 저장된 해당 게임의 점수를 가져오는 코루틴 실행
        yield return StartCoroutine(GetScore(cplayerId, pgameId));

        // 방금 클리어한 게임 스코어와 비교해 현재 스코어가 더 크면 api post 통신으로 score 저장
        if (beforeScore < pscore)
        {
           StartCoroutine(ScoreSave(currentGameId, pscore));
        }
    }

    IEnumerator GetScore(string cmemberId, string pgameId)
    {
        string url = string.Format(getUrl, cmemberId, pgameId);

        // UnityWebRequest Get 요청 생성
        UnityWebRequest www = UnityWebRequest.Get(url);

        // 요청 전송
        yield return www.SendWebRequest();

        // 응답 대기
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            // 연결 오류 시 응답 처리
            print(www.downloadHandler.text);
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Request Error: " + www.error);
        }
        else // 정상 연결되었을 시 응답 처리
        {
            // JSON 데이터를 파싱하여 GameScore 배열로 변환
            string json = www.downloadHandler.text;
            string jsonData = JsonUtility.ToJson(json);

            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Response: " + json);

            // JSON 배열을 감싸는 형식으로 변환
            string wrappedJson = "{\"scores\":" + json + "}";

            // JSON 데이터에서 GameScore 객체 배열로 변환
            GetDataArray wrapper = JsonUtility.FromJson<GetDataArray>(wrappedJson);

            foreach (var score in wrapper.scores) 
            {
                // 받아온 게임 데이터 중 오늘 날짜랑 같은 데이터가 있는지 확인해서 전역변수로 저장
                if(DateTime.Parse(score.gameDate) == DateTime.Now.Date)
                {
                    Debug.Log("Game Id: " + score.gameId);
                    Debug.Log("Game Score: " + score.gameScore);
                    Debug.Log("Game Date: " + score.gameDate);
                    beforeScore = score.gameScore;
                }
            }
        }
    }

    IEnumerator ScoreSave(string pgameId, int pscore)
    {
        // API 통신으로 보낼 데이터 객체 생성
        var scoreData = new PostData
        {
            gameId = pgameId,
            memberId = Login.LoadEncryptedData("userID"),
            gameScore = pscore,
        };

        scoreData.SetGameDate(DateTime.Now.Date);

        // 객체를 JSON 데이터로 변환
        string jsonData = JsonUtility.ToJson(scoreData);

        // JSON 데이터를 UTF-8 인코딩된 바이트 배열로 변환
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // UnityWebRequest 생성 및 설정 (해당 API url에 POST 방식으로 게임 데이터 전송)
        UnityWebRequest www = new UnityWebRequest(postUrl, UnityWebRequest.kHttpVerbPOST);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // 요청 전송
        yield return www.SendWebRequest();

        // 응답 대기
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            // 연결 오류 시 응답 처리
            print(www.downloadHandler.text);
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Request Error: " + www.error);
        }
        else
        {
            // 정상 연결되었을 시 응답 처리
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Response: " + www.downloadHandler.text);
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
