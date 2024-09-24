using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLine : MonoBehaviour {

    public RaycastHit2D downLine;
    public RaycastHit2D leftLine;

    public static bool isLeftLine;
    public static bool isDownLine;

    public LayerMask boxLayer;

    void Start() {
        boxLayer = LayerMask.GetMask("Box");
    }

    void Update() {
        // 현재 오브젝트의 위치에서 Ray를 발사
        Vector2 position = transform.position;

        // Ray를 발사
        downLine = Physics2D.Raycast(position + new Vector2(-2, 0), Vector2.down, 1.1f, boxLayer);
        leftLine = Physics2D.Raycast(position, Vector2.left, 3.1f, boxLayer);

        // Ray가 충돌했는지 여부를 확인하여 bool 값 설정
        if(downLine.collider != null) {
            isDownLine = true;  // 충돌이 있으면 true
        }
        else {
            isDownLine = false; // 충돌이 없으면 false
        }

        if(leftLine.collider != null) {
            isLeftLine = true;  // 충돌이 있으면 true
        }
        else {
            isLeftLine = false; // 충돌이 없으면 false
        }
    }
}