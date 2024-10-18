using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTitleChange : MonoBehaviour
{
    public Sprite[] titleImages;// index [0] storyMode, [1] stageMode
    Image currentImage; //this object image

    // Start is called before the first frame update
    void Start()
    {
        currentImage = GetComponent<Image>();
        StoryOrStage.instance.currentMode = "stage"; //initial setting : stage
        StoryOrStage.instance.modeFlag = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !Login.isNotToggle) //stage
        {
            currentImage.sprite = titleImages[0];
            StoryOrStage.instance.currentMode = "stage";
            StoryOrStage.instance.modeFlag = 0;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !Login.isNotToggle) //story
        {
            currentImage.sprite = titleImages[1];
            StoryOrStage.instance.currentMode = "story";
            StoryOrStage.instance.modeFlag = 1;
        }
    }
}
