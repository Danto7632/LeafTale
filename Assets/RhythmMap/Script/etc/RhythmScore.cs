using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmScore : MonoBehaviour
{
    public GameObject LeftNode;
    public GameObject RightNode;
    public GameObject Clear;

    public int allNodeCount;
    public int allNode;
    public int score;

    public rhythmSoundManager rsm;

    void Start()
    {
        LeftNode = GameObject.Find("LeftNode");
        RightNode = GameObject.Find("RightNode");
        Clear = GameObject.Find("GameClear");

        rsm = GameObject.Find("SoundManager").GetComponent<rhythmSoundManager>();

        allNodeCount = PatternSpawner.rightNodeCount + PatternSpawner.leftNodeCount;
        allNode = allNodeCount;
        score = 0;

    }

    public void NodeHit(int addScore)
    {
        score += addScore;
        allNodeCount--;

        if (allNodeCount == 0)
        {
            Invoke("ScoreUp", 3.0f);
        }
    }

    void ScoreUp()
    {
        float finScore;
        finScore = score * (100.0F / (allNode * 2.0F));
        if(finScore >= 80 && StoryOrStage.instance != null) {
            StoryOrStage.instance.isRhythmGood = true;
            StoryOrStage.instance.clearCount++;
        }
        else if(finScore < 80 && StoryOrStage.instance != null) {
            StoryOrStage.instance.isRhythmGood = false;
        }
        rsm.endSound.Play();
        Clear.GetComponent<GameClear>().Clear((int)(finScore));
    }
}
