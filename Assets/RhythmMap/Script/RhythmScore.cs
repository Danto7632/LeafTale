using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmScore : MonoBehaviour
{
    GameObject LeftNode;
    GameObject RightNode;
    GameObject Clear;
    int leftNodeCount;
    int rightNodeCount;
    public int allNodeCount;
    public int allNode;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        LeftNode = GameObject.Find("LeftNode");
        RightNode = GameObject.Find("RightNode");
        Clear = GameObject.Find("GameClear");
        leftNodeCount = LeftNode.transform.childCount;
        rightNodeCount = RightNode.transform.childCount;

        allNodeCount = leftNodeCount + rightNodeCount;
        allNode = allNodeCount;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NodeHit(int addScore)
    {
        float finScore;
        score += addScore;
        allNodeCount--;
        if (allNodeCount == 0)
        {
            finScore = score * (100.0F / (allNode * 2.0F));
            Clear.GetComponent<GameClear>().Clear((int)(finScore));
        }
    }
}
