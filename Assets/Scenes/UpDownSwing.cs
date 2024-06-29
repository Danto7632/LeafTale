using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class UpDownSwing : MonoBehaviour {
    private LeapServiceProvider leapProvider;
    public Text directionText;

    private Vector3 previousHandPosition;
    private Vector3 previousHandVelocity;

    public float stillThreshold;

    public bool isMoving;
    public Vector3 direction;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        directionText = GameObject.Find("DirectionText").GetComponent<Text>();

        leapProvider.OnUpdateFrame += OnUpdateFrame;

        previousHandPosition = Vector3.zero;
        previousHandVelocity = Vector3.zero;

        stillThreshold = 0.1f;
    }

    void OnDestroy()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            Vector3 currentHandPosition = hand.PalmPosition;
            Vector3 currentHandVelocity = hand.PalmVelocity;

            isMoving = currentHandVelocity.magnitude > stillThreshold;

            direction = currentHandPosition - previousHandPosition;

            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && isMoving) {
                if (direction.y > 0) {
                // 손이 위로 움직이는 경우
                    directionText.text = "위로 움직임";
                }
                else {
                // 손이 아래로 움직이는 경우
                    directionText.text = "아래로 움직임";
                }
            }
            else {
            // 손이 가만히 있거나 좌우로 움직이는 경우
                direction.y = 0;
                directionText.text = "가만히 있음";
            }

            previousHandPosition = currentHandPosition;
            previousHandVelocity = currentHandVelocity;

            Hand hand2 = frame.Hands[0];

        // 손의 방향 벡터
            Vector3 handDirection = hand2.Direction; // Leap Motion의 Direction을 Unity의 Vector3로 변환

        // 위쪽을 기준으로 하는 벡터
            Vector3 upward = Vector3.up;

        // 두 벡터 사이의 각도 계산 (Dot product를 이용)
            float angle = Vector3.Angle(handDirection, upward);

        // 손이 뒤집혔는지 여부 판단 (예시로 90도 이상을 뒤집었다고 가정)
            if (angle > 90f) {
                Debug.Log("손이 뒤집혔습니다.");
            }
            else {
                Debug.Log("손이 뒤집히지 않았습니다.");
            }
        }
    }
}