using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class HandTiltDetection : MonoBehaviour {
    private LeapServiceProvider leapProvider;

    public bool isGameRunning;

    public bool isLeft;
    public bool isRight;
    public bool isUp;
    public bool isDown;

    public Hand hand;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        isGameRunning = false;

        isLeft = false;
        isRight = false;
        isUp = false;
        isDown = false;
    }

    void OnDestroy() {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            hand = frame.Hands[0];

            if(IsFist(hand) && !isGameRunning) {
                Debug.Log("주먹이 감지되었습니다. 1초 후 게임이 시작됩니다.");

                StartCoroutine(RunGame());
            }
        }
        else {
            Debug.Log("손이 감지되지 않았습니다");
            isGameRunning = false;
        }

        DetectHandTilt(hand);
    }

    bool IsFist(Hand hand) {
        return hand.GrabStrength > 0.9f;
    }

    IEnumerator RunGame() {
        yield return new WaitForSeconds(1.0f);

        Debug.Log("시작!");
        isGameRunning = true;
    }

    void DetectHandTilt(Hand hand) {
        if(isGameRunning) {
            Vector3 palmNormal = hand.PalmNormal;

            if (palmNormal.x > 0.5f && !isLeft) {
                Debug.Log("왼쪽으로 기울었습니다.");
                isRight = false;
                isLeft = true;
            }
            else if (palmNormal.x < -0.5f && !isRight) {
                Debug.Log("오른쪽으로 기울었습니다.");
                isLeft = false;
                isRight = true;
            }
            else {
                isLeft = false;
                isRight = false;
            }


            if (palmNormal.z > 0.5f && !isDown) {
                Debug.Log("아래쪽으로 기울었습니다.");
                isUp = false;
                isDown = true;
            }
            else if (palmNormal.z < -0.5f && !isUp) {
                Debug.Log("위쪽으로 기울었습니다.");
                isDown = false;
                isUp = true;
            }
            else {
                isDown = false;
                isUp = false;
            }
        }
    }
}