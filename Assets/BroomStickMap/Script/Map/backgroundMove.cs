using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public broomMove broomStatus;

    public Transform[] sprites;

    public int startIndex;
    public int endIndex;
    public float speed;
    public float viewHeight;


    public void Awake() {
        broomStatus = GameObject.FindWithTag("Player").GetComponent<broomMove>();

        speed = 10;
        viewHeight = Camera.main.orthographicSize * 2;
    }

    void Update() {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (sprites[endIndex].position.y < viewHeight * (-1) - 30 && !broomStatus.isGameOver) {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;

            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * 36;

            int startIndexSave = startIndex;

            startIndex = endIndex;
            endIndex = (startIndexSave - 1 == -1) ? sprites.Length - 1 : startIndexSave - 1;
        }
    }
}
