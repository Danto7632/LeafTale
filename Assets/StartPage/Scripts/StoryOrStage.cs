using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryOrStage : MonoBehaviour
{
    public static StoryOrStage instance { get; private set; }

    public string currentMode;
    public string nextStory;

    public bool G001Cleared = false;
    public bool G002Cleared = false;
    public bool G003Cleared = false;
    public bool G004Cleared = false;
    public bool G005Cleared = false;

    public int G001Score = 0;
    public int G002Score = 0;
    public int G003Score = 0;
    public int G004Score = 0;
    public int G005Score = 0;

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

    public bool AllGamesCleared()
    {
        return G001Cleared && G002Cleared && G003Cleared && G004Cleared && G005Cleared;
    }
}
