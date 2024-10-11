using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    public Toggle stageToggle;
    public Toggle storyToggle;

    void Start()
    {
        // Toggle 이벤트에 메소드 연결
        stageToggle.onValueChanged.AddListener(OnStageToggleChanged);
        storyToggle.onValueChanged.AddListener(OnStoryToggleChanged);

        // 초기 상태 설정
        if (stageToggle.isOn)
        {
            StoryOrStage.instance.currentMode = "stage";
            StoryOrStage.instance.modeFlag = 0;  // 스테이지 모드
        }
        else if (storyToggle.isOn)
        {
            StoryOrStage.instance.currentMode = "story";
            StoryOrStage.instance.modeFlag = 1;  // 스토리 모드
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !Login.isNotToggle) {
            stageToggle.isOn = true;
            storyToggle.isOn = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !Login.isNotToggle){
            storyToggle.isOn = true;
            stageToggle.isOn = false;
        }

        if (Login.isNotToggle) {
            storyToggle.interactable = false;
            stageToggle.interactable = false;
        }
    }

    void OnStageToggleChanged(bool isOn)
    {
        if (isOn && !Login.isNotToggle)
        {
            StoryOrStage.instance.currentMode = "stage";
            StoryOrStage.instance.modeFlag = 0;  // 스테이지 모드
            storyToggle.isOn = false;
        }
    }

    void OnStoryToggleChanged(bool isOn)
    {
        if (isOn && !Login.isNotToggle)
        {
            StoryOrStage.instance.currentMode = "story";
            StoryOrStage.instance.modeFlag = 1;  // 스토리 모드
            stageToggle.isOn = false;
        }
    }
}
