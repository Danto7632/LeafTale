using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    public Toggle stageToggle;
    public Toggle storyToggle;

    void Start()
    {
        // 체크박스 상태 변경 이벤트에 메서드 연결
        stageToggle.onValueChanged.AddListener(OnStageToggleChanged);
        storyToggle.onValueChanged.AddListener(OnStoryToggleChanged);

        // 초기 상태 설정
        if (stageToggle.isOn)
        {
            StoryOrStage.instance.currentMode = "stage";
        }
        else if (storyToggle.isOn)
        {
            StoryOrStage.instance.currentMode = "story";
        }
    }

    void OnStageToggleChanged(bool isOn)
    {
        if (isOn)
        {
            StoryOrStage.instance.currentMode = "stage";
            storyToggle.isOn = false; // Story 체크박스는 해제
        }
    }

    void OnStoryToggleChanged(bool isOn)
    {
        if (isOn)
        {
            StoryOrStage.instance.currentMode = "story";
            stageToggle.isOn = false; // Stage 체크박스는 해제
        }
    }
}