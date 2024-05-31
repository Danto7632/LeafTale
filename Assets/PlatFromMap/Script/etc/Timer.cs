using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 클래스를 사용하려면 이 네임스페이스를 추가하세요

public class Timer : MonoBehaviour
{
    public TMP_Text[] timeText;
    public TMP_Text gameOverText;
    float time = 120f; // 제한 시간 120초
    int min, sec;
    
    void Start()
    {
        // timeText 배열이 null이 아니고 크기가 2 이상인지 확인
        if (timeText != null && timeText.Length >= 2)
        {
            // 제한 시간 02:00으로 초기화
            timeText[0].text = "02";
            timeText[1].text = "00";
        }
        else
        {
            Debug.LogError("timeText 배열이 null이거나 크기가 2보다 작습니다.");
        }
    }
    
    void Update()
    { 
        time -= Time.deltaTime;

        min = (int)time / 60;
        sec = (int)time % 60;

        if (time <= 0)
        {
            time = 0;
            gameOverText.text = "게임 오버"; // 게임 오버 텍스트를 표시합니다
            // 추가로 업데이트를 비활성화하거나 게임 오버 로직을 처리할 수 있습니다
        }

        // 타이머 텍스트 업데이트
        if (timeText != null && timeText.Length >= 2)
        {
            timeText[0].text = min.ToString("00");
            timeText[1].text = sec.ToString("00");
        }
    }
}
