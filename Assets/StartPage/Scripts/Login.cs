using System;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using EasyTransition;

public class Login : MonoBehaviour {
    private TMP_Text pressStartText;
    private Camera mainCamera;

    public GameObject rank;
    public GameObject pressStart;
    public GameObject backGroundPanel;

    public Animator backAni;

    [Header("Login")]
    public GameObject loginPanel;
    public GameObject MainTitle;
    public TMP_InputField idInput;
    public TMP_InputField pwInput;
    public UnityEngine.UI.Button loginButton;
    public TMP_Text loginFail;

    private bool isFadingOut;
    public static bool isNotToggle;
    private string url;
    
    private static readonly byte[] key = Encoding.UTF8.GetBytes("Id28e3PsN258b1R4");
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("D5oC29U1v94eBp7m");

    public TransitionSettings transition_B;

    private bool isTransitionTriggered = false;

    public string nextSceneName;

    [System.Serializable]
    public class LoginData
    {
        public string memberId;
        public string password;
    }

    public static bool isAlreadyLogin;

    void Start()
    {
        MainTitle = GameObject.Find("MainTitle");
        backGroundPanel = GameObject.Find("Background");
        backAni = backGroundPanel.GetComponent<Animator>();
        url = "http://43.203.0.69:8080/api/login";
        mainCamera = Camera.main;
        isFadingOut = false;
        string savedId = LoadEncryptedData("userID");
        string savedPassword = LoadEncryptedData("userPassword");

        if (!string.IsNullOrEmpty(savedId) && !string.IsNullOrEmpty(savedPassword)) {
            isAlreadyLogin = true; 
            
            idInput.text = savedId; // ID 필드에 자동으로 입력
            pwInput.text = savedPassword; // 비밀번호 필드에 자동으로 입력 (선택적)

            // 자동으로 로그인 시도 (원하는 경우)
            StartCoroutine(AccountLogin());

        }
        else {
            isAlreadyLogin = false;
        }

        isNotToggle = false;

        backAni.enabled = false;

        isTransitionTriggered = false;
    }

    void Update() {
        if (StoryOrStage.instance != null) {
            if (pressStart.activeSelf == true && Input.GetMouseButtonDown(0) && StoryOrStage.instance.currentMode == "story") LoadSceneAfterDelay("StoryPage");
            if (pressStart.activeSelf == true && Input.GetMouseButtonDown(0) && StoryOrStage.instance.currentMode == "stage") LoadSceneAfterDelay("StageSelect");
            
            if(pressStart.activeSelf == true && Input.GetMouseButtonDown(0) && StoryOrStage.instance.currentMode == null) {
                Debug.Log("Select Story OR Stage");
            }
        }

        if (backAni != null && backAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !isTransitionTriggered)
        {
            // 애니메이션이 끝났고 트랜지션이 아직 실행되지 않았을 때
            isTransitionTriggered = true;
            TransitionManager.Instance().Transition(nextSceneName, transition_B, 0);
        }
    }
    // 로그인 버튼 누르면 API 통신으로 로그인 진행되는 코루틴 실행 
    public void LoginBtn() 
    {
        StartCoroutine(AccountLogin());
    }

    IEnumerator AccountLogin()
    {
        // API 통신으로 보낼 데이터 객체 생성
        var loginData = new LoginData
        {
            memberId = idInput.text,
            password = pwInput.text
        };

        // 객체를 JSON 데이터로 변환
        string jsonData = JsonUtility.ToJson(loginData);

        // JSON 데이터를 UTF-8 인코딩된 바이트 배열로 변환
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // UnityWebRequest 생성 및 설정 (해당 API url에 POST 방식으로 아이디와 비밀번호 전송)
        UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // 요청 전송
        yield return www.SendWebRequest();

        // 응답 처리
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            // 연결 오류 시 응답 처리
            loginFail.gameObject.SetActive(true);
            loginFail.text = "Connection Error";
            pwInput.text = "";
            print(www.downloadHandler.text);
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Request Error: " + www.error);
        }
        else
        {
            // 정상 연결되었을 시 응답 처리
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Response: " + www.downloadHandler.text);
            if (www.responseCode == 200 && www.downloadHandler.text == "true")
            {
                loginFail.gameObject.SetActive(false);
                // 아이디 암호화해서 로컬 저장하는 사용자 지정 함수 (점수 데이터 전송할 때 같이 전송하기 위함)
                SaveEncryptedData("userID", idInput.text); 
                SaveEncryptedData("userPassword", pwInput.text); // 비밀번호도 저장

                loginPanel.SetActive(false);
                rank.SetActive(false);
                pressStart.SetActive(true);
                StartCoroutine(BlinkText());
            }
            else if (www.responseCode == 200 && www.downloadHandler.text != "true")
            {
                loginFail.gameObject.SetActive(true);
                loginFail.text = "아이디나 비밀번호를 잘못 입력하였습니다.\n아이디와 비밀번호를 올바르게 입력해주세요.";
                pwInput.text = "";
            }
        }
    }

    IEnumerator BlinkText() {
        pressStartText = pressStart.GetComponent<TMP_Text>();
        Color originalColor = pressStartText.color;

        float alpha = 1f;

        while (true) {
            if (isFadingOut) alpha -= Time.deltaTime;
            else alpha += Time.deltaTime;

            alpha = Mathf.Clamp01(alpha);
            pressStartText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            if (alpha >= 1f) {
                isFadingOut = true;
                yield return new WaitForSeconds(0.1f);
            }
            else if (alpha <= 0f) {
                isFadingOut = false;
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
    }

    void LoadSceneAfterDelay(string sceneName) {
        backAni.enabled = true;
        pressStart.SetActive(false);

        MainTitle.SetActive(false);
        StoryOrStage.instance.nextStory = "Explain";

        nextSceneName = sceneName;
    }

    public static void SaveEncryptedData(string keyName, string data) // 아이디 암호화해서 저장
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            // 암호화 키와 초기화 벡터를 이용하여, 암호화를 진행할 encryptor 생성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            byte[] encryptedData = null;

            // 일반 데이터를 암호화
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(data);
            encryptedData = encryptor.TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);

            // 암호화 데이터를 문자열로 변환하여 저장 
            string encryptedString = Convert.ToBase64String(encryptedData);
            PlayerPrefs.SetString(keyName, encryptedString);
            PlayerPrefs.Save();
        }
    }
    public static string LoadEncryptedData(string keyName) // 아이디 복호화 후 불러오기
    {
        string encryptedString = PlayerPrefs.GetString(keyName);
        if (!string.IsNullOrEmpty(encryptedString))
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                //암호화 키와 초기화 벡터를 이용하여 복호화를 진행할 decryptor 생성
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // 데이터 복호화
                byte[] encryptedData = Convert.FromBase64String(encryptedString);
                byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                // 복호화된 데이터를 이용하여 저장된 데이터 반환
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        else
        {
            return null;
        }
    }
}
