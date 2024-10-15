using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryOrStage : MonoBehaviour
{
    public static StoryOrStage instance { get; private set; }

    public bool isClawGood = false;
    public bool isBroomGood = false;
    public bool isPlatGood = false;
    public bool isRhythmGood = false;
    public bool isMagicGood = false;

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

    // 스토리와 스테이지 모드를 구분하는 플래그 추가
    public int modeFlag = 0;

    public int clearCount = 0;

    private void Awake()
    {
        // Singleton 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경 시 오브젝트가 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스가 생성되면 파괴
        }

        isClawGood = false;
        isBroomGood = false;
        isPlatGood = false;
        isRhythmGood = false;
        isMagicGood = false;

        G001Cleared = false;
        G002Cleared = false;
        G003Cleared = false;
        G004Cleared = false;
        G005Cleared = false;

        G001Score = 0;
        G002Score = 0;
        G003Score = 0;
        G004Score = 0;
        G005Score = 0;
        modeFlag = 0;

        clearCount = 0;
    }

    void Start() {
        Application.targetFrameRate = 60;
    }

    public bool AllGamesCleared()
    {
        return G001Cleared && G002Cleared && G003Cleared && G004Cleared && G005Cleared;
    }

    public void ResetGameStatus()
    {
        G001Cleared = false;
        G002Cleared = false;
        G003Cleared = false;
        G004Cleared = false;
        G005Cleared = false;
        
        G001Score = 0;
        G002Score = 0;
        G003Score = 0;
        G004Score = 0;
        G005Score = 0;
    }
}
