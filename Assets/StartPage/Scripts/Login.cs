using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Login : MonoBehaviour {
    private TMP_Text pressStartText;
    private Camera mainCamera;

    public GameObject rank;
    public GameObject pressStart;

    [Header("Login")]
    public GameObject loginPanel;
    public TMP_InputField idInput;
    public TMP_InputField pwInput;
    public UnityEngine.UI.Button loginButton;
    public TMP_Text loginFail;

    private TMP_Text pressStartText;
    private bool isFadingOut;
    private Camera mainCamera;

    private string url;

    [System.Serializable]
    public class LoginData
    {
        public string memberId;
        public string password;
    }

    void Start()
    {
        url = "http://43.203.0.69:8080/api/login";
        mainCamera = Camera.main;
        isFadingOut = false;
    }

    void Update() {
        if (pressStart.activeSelf == true && Input.GetMouseButtonDown(0)) StartCoroutine(LoadSceneAfterDelay("StoryPage", 3f));
    }

    public void LoginBtn()
    {
        StartCoroutine(AccountLogin());
    }

    IEnumerator AccountLogin()
    {
        var loginData = new LoginData
        {
            memberId = idInput.text,
            password = pwInput.text
        };

        string jsonData = JsonUtility.ToJson(loginData);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            loginFail.gameObject.SetActive(true);
            loginFail.text = "Connection Error";
            idInput.text = "";
            pwInput.text = "";
            print(www.downloadHandler.text);
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Request Error: " + www.error);
        }
        else
        {
            Debug.Log("Response Code: " + www.responseCode);
            Debug.Log("Response: " + www.downloadHandler.text);
            if (www.responseCode == 200 && www.downloadHandler.text == "true")
            {
                loginFail.gameObject.SetActive(false);
                PlayerPrefs.SetString("userID", idInput.text);
                PlayerPrefs.Save();

                loginPanel.SetActive(false);
                rank.SetActive(true);
                pressStart.SetActive(true);
                StartCoroutine(BlinkText());
            }
            else if (www.responseCode == 200 && www.downloadHandler.text != "true")
            {
                loginFail.gameObject.SetActive(true);
                loginFail.text = "아이디나 비밀번호를 잘못 입력하였습니다.\n아이디와 비밀번호를 올바르게 입력해주세요.";
                idInput.text = "";
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

    IEnumerator LoadSceneAfterDelay(string sceneName, float delay) {
        pressStart.SetActive(false);

        yield return StartCoroutine(anima(delay / 3, -3f, -1f, 4f));
        yield return StartCoroutine(anima(delay / 3, 2f, 1f, 3f));
        yield return StartCoroutine(anima(delay / 3, -1f, -1f, 2f));

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator anima(float duration, float x, float y, float size) {
        float elapsedTime = 0f;
        float initialSize = mainCamera.orthographicSize;
        Vector3 initialPosition = mainCamera.transform.position;

        float targetSize = size;
        Vector3 targetPosition = new Vector3(x, y, initialPosition.z);

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float shakeAmount = 0.03f;
            Vector3 shakeOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);

            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            mainCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t) + shakeOffset;

            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = targetPosition;
    }
}
