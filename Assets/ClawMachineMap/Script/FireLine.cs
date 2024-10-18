using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLine : MonoBehaviour {

    public RaycastHit2D[] downLine;
    public RaycastHit2D leftLine;
    public RaycastHit2D rightLine;

    public static bool isLeftLine;
    public static bool isDownLine;
    public static bool isRightLine;

    public LayerMask boxLayer;

    void Start() {
        boxLayer = LayerMask.GetMask("wall");
        downLine = new RaycastHit2D[11];
    }

    void Update() {
        // 현재 오브젝트의 위치에서 Ray를 발사
        Vector2 position = transform.position;
        Vector2 downPosition = (Vector2)transform.position + new Vector2(-3, 0);

        // 아래쪽 Ray를 발사
        for (int i = 0; i < 11; i++) {
            downLine[i] = Physics2D.Raycast(downPosition, Vector2.down, 1.1f, boxLayer);
        
            // Ray 시각화
            Debug.DrawRay(downPosition, Vector2.down * 1.1f, Color.red);

            if (downLine[i].collider != null) {
                isDownLine = true; // 하나라도 충돌이 있으면 true로 설정
                break;
            }
            else {
                isDownLine = false; // 초기화
            }
            downPosition += new Vector2(0.5f, 0);
        }

        // 왼쪽 Ray를 발사 및 시각화
        leftLine = Physics2D.Raycast(position, Vector2.left, 3.1f, boxLayer);
        Debug.DrawRay(position, Vector2.left * 3.1f, Color.green);

        // 오른쪽 Ray를 발사 및 시각화
        rightLine = Physics2D.Raycast(position, Vector2.right, 3.1f, boxLayer);
        Debug.DrawRay(position, Vector2.right * 3.1f, Color.blue);

        // Ray가 충돌했는지 여부를 확인하여 bool 값 설정
        isLeftLine = leftLine.collider != null;
        isRightLine = rightLine.collider != null;
    }
}