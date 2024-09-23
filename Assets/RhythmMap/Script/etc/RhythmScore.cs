using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmScore : MonoBehaviour
{
    public GameObject LeftNode;
    public GameObject RightNode;
    public GameObject Clear;

    public int leftNodeCount;
    public int rightNodeCount;
    public int allNodeCount;
    public int allNode;
    public int score;

    private PatternSpawner patternSpawner;

    void Start()
    {
        LeftNode = GameObject.Find("LeftNode");
        RightNode = GameObject.Find("RightNode");
        Clear = GameObject.Find("GameClear");

        patternSpawner = GameObject.Find("PatternSpawner").GetComponent<PatternSpawner>();

        leftNodeCount = CountValidNodes(patternSpawner.LeftPattern);
        rightNodeCount = CountValidNodes(patternSpawner.RightPattern);

        allNodeCount = leftNodeCount + rightNodeCount;
        allNode = allNodeCount;
        score = 0;
        
        allNode = 18;
        allNodeCount = 18;
    }

    int CountValidNodes(List<int> pattern)
    {
        int count = 0;
        foreach (int value in pattern)
        {
            if (value != 0)
            {
                count++;
            }
        }
        return count;
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
        Clear.GetComponent<GameClear>().Clear((int)(finScore));
        try
        {
            EndingValue.EndingSet(2);
        }
        catch
        {
            Debug.Log("EndingValue 오브젝트에 값을 전달하지 못했습니다.");
        }
        
    }
}
