using UnityEngine;
using Leap;
using Leap.Unity;

public class HandDirectionDetection : MonoBehaviour
{
    private LeapServiceProvider leapProvider;
    private Vector3 previousLeftHandPosition = Vector3.zero;
    private Vector3 previousRightHandPosition = Vector3.zero;

    private bool isLeftMovingUp = false;
    private bool isLeftMovingDown = false;
    private bool isRightMovingUp = false;
    private bool isRightMovingDown = false;

    private bool isHandFlippedDown = false;
    private bool isHandFlippedUp = false;

    private bool shouldPrintLURD = false; // 왼손이 위로, 오른손이 아래로 움직임을 출력해야 할지 여부
    private bool shouldPrintLDRU = false; // 왼손이 아래로, 오른손이 위로 움직임을 출력해야 할지 여부

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void OnUpdateFrame(Frame frame) {
        catMovingUPDOWN(frame);
        catJumpFlip(frame);
        catFlipClap(frame);
    }

    void catMovingUPDOWN(Frame frame) {
        foreach (Hand hand in frame.Hands) {
            Vector3 currentHandPosition = hand.PalmPosition;
            Vector3 previousHandPosition = hand.IsLeft ? previousLeftHandPosition : previousRightHandPosition;

            if (previousHandPosition != Vector3.zero) {
                Vector3 handDirection = (currentHandPosition - previousHandPosition).normalized;
                float handSpeed = hand.PalmVelocity.magnitude;

                if (handSpeed > 5f) {
                    if (hand.IsLeft) {
                        if (Mathf.Abs(handDirection.y) > Mathf.Abs(handDirection.x)) {
                            if (handDirection.y > 0) {
                                isLeftMovingUp = true; // 왼손이 위로
                                isLeftMovingDown = false;
                            }
                            else {
                                isLeftMovingDown = true; // 왼손이 아래로
                                isLeftMovingUp = false;
                            }
                        }
                    }
                    else if (hand.IsRight) {
                        if (Mathf.Abs(handDirection.y) > Mathf.Abs(handDirection.x)) {
                            if (handDirection.y > 0) {
                                isRightMovingUp = true; // 오른손이 위로
                                isRightMovingDown = false;
                            }
                            else {
                                isRightMovingDown = true; // 오른손이 아래로
                                isRightMovingUp = false;
                            }
                        }
                    }
                }
            }
            else {
                if (hand.IsLeft) {
                    previousLeftHandPosition = currentHandPosition;
                }
                else if (hand.IsRight) {
                    previousRightHandPosition = currentHandPosition;
                }
            }
        }

        // 번갈아가며 움직일 때 콘솔 출력
        if (isLeftMovingUp && isRightMovingDown && !shouldPrintLURD) {
            Debug.Log("왼손이 위로, 오른손이 아래로 움직임");
            shouldPrintLURD = true;
            shouldPrintLDRU = false;
        }
        else if (isLeftMovingDown && isRightMovingUp && !shouldPrintLDRU) {
            Debug.Log("왼손이 아래로, 오른손이 위로 움직임");
            shouldPrintLURD = false;
            shouldPrintLDRU = true;
        }

        // 움직임이 멈추면 상태 초기화
        if (!(isLeftMovingUp || isLeftMovingDown)) {
            shouldPrintLURD = false;
        }
        if (!(isRightMovingUp || isRightMovingDown)) {
            shouldPrintLDRU = false;
        }

        // 모든 움직임 상태 초기화
        isLeftMovingUp = false;
        isLeftMovingDown = false;
        isRightMovingUp = false;
        isRightMovingDown = false;
    }
    void catJumpFlip(Frame frame) {
        foreach (Hand hand in frame.Hands) {
            Vector3 palmNormal = hand.PalmNormal;

            if (palmNormal.y > 0.5f && !isHandFlippedDown)
            {
                Debug.Log("손바닥이 위에서 아래로 향함");
                isHandFlippedDown = true;
                isHandFlippedUp = false;
            }
            else if (palmNormal.y < -0.5f && !isHandFlippedUp)
            {
                Debug.Log("손바닥이 아래에서 위로 향함");
                isHandFlippedDown = false;
                isHandFlippedUp = true;
            }
        }
    }

    void catFlipClap(Frame frame) {
        Hand leftHand = null;
        Hand rightHand = null;

        foreach (Hand hand in frame.Hands) {
            if (hand.IsLeft)
                leftHand = hand;
            else if (hand.IsRight)
                rightHand = hand;
        }

        if (leftHand != null && rightHand != null) {
            Vector3 leftPalmPosition = leftHand.PalmPosition;
            Vector3 rightPalmPosition = rightHand.PalmPosition;

            float clapDistanceThreshold = 0.5f;
            float handsDistance = Vector3.Distance(leftPalmPosition, rightPalmPosition);

            if (handsDistance < clapDistanceThreshold) {
                Debug.Log("박수 감지!");
            }
        }
    }
}