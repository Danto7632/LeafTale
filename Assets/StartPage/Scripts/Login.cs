using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public GameObject rank;
    public GameObject pressStart;

    [Header("Login")]
    public GameObject loginPanel;
    public UnityEngine.UI.Button loginButton;

    private TMP_Text pressStartText;
    private bool isFadingOut = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (pressStart.activeSelf == true && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LoadSceneAfterDelay("StoryPage", 3f));
        }
    }

    public void LoginBtn()
    {
        loginPanel.SetActive(false);
        rank.SetActive(true);
        pressStart.SetActive(true);
        StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        pressStartText = pressStart.GetComponent<TMP_Text>();
        Color originalColor = pressStartText.color;
        float alpha = 1f;

        while (true)
        {
            if (isFadingOut)
            {
                alpha -= Time.deltaTime;
            }
            else
            {
                alpha += Time.deltaTime;
            }

            alpha = Mathf.Clamp01(alpha);
            pressStartText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            if (alpha >= 1f)
            {
                isFadingOut = true;
                yield return new WaitForSeconds(0.1f); // 0.1초 대기
            }
            else if (alpha <= 0f)
            {
                isFadingOut = false;
                yield return new WaitForSeconds(0.1f); // 0.1초 대기
            }

            yield return null;
        }
    }

    IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        pressStart.SetActive(false); // Press Start를 invisible로 만듦

        // ani1, ani2, ani3 코루틴을 순차적으로 실행
        yield return StartCoroutine(anima(delay / 3, -3f, -1f, 4f));
        yield return StartCoroutine(anima(delay / 3, 2f, 1f, 3f));
        yield return StartCoroutine(anima(delay / 3, -1f, -1f, 2f));

        SceneManager.LoadScene(sceneName); // 다음 씬으로 전환
    }

    IEnumerator anima(float duration, float x, float y, float size)
    {
        float elapsedTime = 0f;
        float initialSize = mainCamera.orthographicSize;
        Vector3 initialPosition = mainCamera.transform.position;
        float targetSize = size;
        Vector3 targetPosition = new Vector3(x, y, initialPosition.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 떨림 효과 추가
            float shakeAmount = 0.03f; // 떨림 강도
            Vector3 shakeOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);

            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            mainCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t) + shakeOffset;

            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = targetPosition;
    }
}
