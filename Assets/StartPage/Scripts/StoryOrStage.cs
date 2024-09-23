using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryOrStage : MonoBehaviour
{
    public static StoryOrStage instance { get; private set; }

    public string currentMode;
    public string nextStory;
    
    private void Awake()
    {
        // 싱글턴 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 간에도 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 이 객체는 파괴
        }
    }
}