using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class LeapSwing : MonoBehaviour
{
    private LeapServiceProvider leapProvider;

    private Vector3 previousHandPosition;
    private Vector3 previousHandVelocity;

    public float stillThreshold;

    public GameObject Player;
    public broomMove broomStatus;

    public bool isMoving;
    public Vector3 direction;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();

        leapProvider.OnUpdateFrame += OnUpdateFrame;

        previousHandPosition = Vector3.zero;
        previousHandVelocity = Vector3.zero;

        Player = GameObject.FindWithTag("Player");
        broomStatus = Player.GetComponent<broomMove>();

        stillThreshold = 0.1f;
    }

    void OnDestroy()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame)
    {
        if (frame.Hands.Count > 0)
        {
            Hand hand = frame.Hands[0];

            Vector3 currentHandPosition = hand.PalmPosition;
            Vector3 currentHandVelocity = hand.PalmVelocity;

            if(currentHandVelocity.magnitude >= 0.8f) {
                isMoving = currentHandVelocity.magnitude > stillThreshold;

                direction = currentHandPosition - previousHandPosition;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && isMoving)
                {
                    if (direction.x > 0) {
                        broomStatus.isRight = true;
                        broomStatus.isLeft = false;
                    }
                    else if (direction.x < 0) {
                        broomStatus.isRight = false;
                        broomStatus.isLeft = true;
                    }
                }

                // 손의 움직임 속도를 콘솔에 출력
                Debug.Log("Hand Velocity: " + currentHandVelocity.magnitude);

                previousHandPosition = currentHandPosition;
                previousHandVelocity = currentHandVelocity;
            }
        }
        else {
            broomStatus.isRight = false;
            broomStatus.isLeft = false;
            direction.x = 0;
        }
    }
}