using UnityEngine;

public class PositionLimiter : MonoBehaviour
{
    public Vector3 minLimit = new Vector3(-8, -4.5f, 0); // 최소 위치
    public Vector3 maxLimit = new Vector3(8, 4.5f, 0);  // 최대 위치

    void Update()
    {
        // 현재 오브젝트의 위치
        Vector3 position = transform.position;

        // X, Y, Z 좌표를 개별적으로 제한
        position.x = Mathf.Clamp(position.x, minLimit.x, maxLimit.x);
        position.y = Mathf.Clamp(position.y, minLimit.y, maxLimit.y);
        position.z = Mathf.Clamp(position.z, minLimit.z, maxLimit.z);

        // 제한된 위치로 오브젝트의 위치 업데이트
        transform.position = position;
    }
}