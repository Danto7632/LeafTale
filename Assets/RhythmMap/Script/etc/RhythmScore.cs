using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmScore : MonoBehaviour {
    public GameObject LeftNode;
    public GameObject RightNode;
    public GameObject Clear;

    public int leftNodeCount;
    public int rightNodeCount;
    public int allNodeCount;
    public int allNode;
    public int score;

    void Start() {
        LeftNode = GameObject.Find("LeftNode");
        RightNode = GameObject.Find("RightNode");
        Clear = GameObject.Find("GameClear");

        leftNodeCount = LeftNode.transform.childCount;
        rightNodeCount = RightNode.transform.childCount;

        allNodeCount = leftNodeCount + rightNodeCount;
        allNode = allNodeCount;
        score = 0;
    }

    public void NodeHit(int addScore) {
        float finScore;

        score += addScore;
        allNodeCount--;

        if (allNodeCount == 0) {
            finScore = score * (100.0F / (allNode * 2.0F));
            Clear.GetComponent<GameClear>().Clear((int)(finScore));
        }
    }
}
