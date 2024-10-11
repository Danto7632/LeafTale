using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    public Toggle stageToggle;
    public Toggle storyToggle;

    public AudioSource changedSound;

    void Start()
    {

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

    void Update() {
        if(Input.GetKeyDown(KeyCode.LeftArrow) && !Login.isNotToggle) {
            stageToggle.isOn = true;
            storyToggle.isOn = false;
            changedSound.Play();
        }

        else if(Input.GetKeyDown(KeyCode.RightArrow) && !Login.isNotToggle){
            storyToggle.isOn = true;
            stageToggle.isOn = false;
            changedSound.Play();
        }

        if(Login.isNotToggle) {
            storyToggle.interactable = false;
            stageToggle.interactable = false;
        }
    }
}