using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkSelf : MonoBehaviour {
    private Image imageComponent; // 자신의 Image 컴포넌트
    public float blinkDuration;

    private void Start() {
        blinkDuration = 0.2f;
        imageComponent = GetComponent<Image>(); // 자신의 Image 컴포넌트를 가져옴
        StartCoroutine(BlinkImage());
    }

    private IEnumerator BlinkImage() {
        while (true) { // 무한 루프
            imageComponent.enabled = !imageComponent.enabled; // Image의 활성화 상태 토글
            yield return new WaitForSeconds(blinkDuration); // 주기만큼 대기
        }
    }
}