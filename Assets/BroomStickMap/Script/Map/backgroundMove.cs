using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public broomMove broomStatus;

    public Transform[] sprites;

    public int startIndex;
    public int endIndex;
    public float speed = 10;
    public float viewHeight;


    public void Awake() {
        broomStatus = GameObject.FindWithTag("Player").GetComponent<broomMove>();
        viewHeight = 22.0f;
    }

    void Update() {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (sprites[endIndex].position.y < viewHeight * (-1) && !broomStatus.isGameOver) {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;

            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * 31.3f;

            int startIndexSave = startIndex;

            startIndex = endIndex;
            endIndex = startIndexSave;
        }
    }
}
