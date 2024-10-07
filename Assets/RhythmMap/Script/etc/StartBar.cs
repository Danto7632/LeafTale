using UnityEngine;
using UnityEngine.UI;

public class StartBar : MonoBehaviour {
    public static Image barImage;
    public static RectTransform endImageTransform;
    
    private static float animationDuration = 3f; // 애니메이션 지속 시간
    private static float targetPosition = 150f; // 목표 위치

    void Start() {
        barImage = GameObject.Find("StartBar").GetComponent<Image>();
        endImageTransform = GameObject.Find("EndImage").GetComponent<RectTransform>();

        // EndImage 초기 위치 설정
        UpdateEndImagePosition(-150f); // 초기 위치를 -150으로 설정
    }

    public static void ChangeHealthBarAmount(float elapsedTime) {
        float fillAmount = elapsedTime / animationDuration; // elapsedTime을 애니메이션 지속 시간으로 나누기
        barImage.fillAmount = fillAmount;

        // EndImage 위치 업데이트
        UpdateEndImagePosition(fillAmount); // fillAmount를 사용하여 위치 결정

        // fillAmount에 따라 health bar의 활성화 여부 결정
        barImage.enabled = fillAmount > 0f && fillAmount < 1f;
    }

    private static void UpdateEndImagePosition(float fillAmount) {
        // -150에서 150까지 비율에 따라 보간
        float endImagePosition = Mathf.Lerp(-150f, targetPosition, fillAmount); 
        endImageTransform.anchoredPosition = new Vector2(endImagePosition, endImageTransform.anchoredPosition.y);
    }

    // 호출할 메서드 추가: 목표 위치로 애니메이션
    public static void AnimateEndImageToTarget() {
        float elapsedTime = 0f;
        float initialPosition = endImageTransform.anchoredPosition.x;

        while (elapsedTime < animationDuration) {
            float t = elapsedTime / animationDuration;
            float newPosition = Mathf.Lerp(initialPosition, targetPosition, t);
            endImageTransform.anchoredPosition = new Vector2(newPosition, endImageTransform.anchoredPosition.y);
            elapsedTime += Time.deltaTime;
        }

        endImageTransform.anchoredPosition = new Vector2(targetPosition, endImageTransform.anchoredPosition.y); // 마지막 위치 보정
    }
}