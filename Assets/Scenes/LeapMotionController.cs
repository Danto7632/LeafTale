using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapMotionController : MonoBehaviour {
    public GameObject TileCheckerL; // 왼손 타일 체커
    public GameObject TileCheckerR; // 오른손 타일 체커

    private LeapServiceProvider leapProvider;
    private TileCheck tileL;
    private TileCheck tileR;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();

        // 각 타일 체커를 찾아서 연결
        TileCheckerL = GameObject.FindWithTag("tileCheckL");
        TileCheckerR = GameObject.FindWithTag("tileCheckR");

        // 타일 체커의 TileCheck 스크립트 참조
        tileL = TileCheckerL.GetComponent<TileCheck>();
        tileR = TileCheckerR.GetComponent<TileCheck>();
    }

    void Update() {
        Frame frame = leapProvider.CurrentFrame;

        // 각 손에 대해 동작 감지 및 처리
        foreach (Hand hand in frame.Hands) {
            if (hand.IsLeft) {
                DetectLeftHandPose(hand); // 왼손 동작 처리
            } else if (hand.IsRight) {
                DetectRightHandPose(hand); // 오른손 동작 처리
            }
        }
    }

    void DetectLeftHandPose(Hand hand) {
        bool isFist = true;
        bool isPalm = true;
        bool isScissors = false;

        // 가위 인식
        if (hand.Fingers[1].IsExtended && hand.Fingers[2].IsExtended &&
            !hand.Fingers[0].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended) {
            isScissors = true;
        }

        // 주먹 인식
        foreach (Finger finger in hand.Fingers) {
            if (finger.IsExtended) {
                isFist = false;
            }
        }

        // 평평한 손바닥 인식
        foreach (Finger finger in hand.Fingers) {
            if (!finger.IsExtended) {
                isPalm = false;
            }
        }

        // 왼손 타일 체커에 동작 전달
        if (isFist) {
            tileL.getLeap("ROCK");
        } else if (isPalm) {
            tileL.getLeap("PAPER");
        } else if (isScissors) {
            tileL.getLeap("SCISSOR");
        }
    }

    void DetectRightHandPose(Hand hand) {
        bool isFist = true;
        bool isPalm = true;
        bool isScissors = false;

        // 가위 인식
        if (hand.Fingers[1].IsExtended && hand.Fingers[2].IsExtended &&
            !hand.Fingers[0].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended) {
            isScissors = true;
        }

        // 주먹 인식
        foreach (Finger finger in hand.Fingers) {
            if (finger.IsExtended) {
                isFist = false;
            }
        }

        // 평평한 손바닥 인식
        foreach (Finger finger in hand.Fingers) {
            if (!finger.IsExtended) {
                isPalm = false;
            }
        }

        // 오른손 타일 체커에 동작 전달
        if (isFist) {
            tileR.getLeap("ROCK");
        } else if (isPalm) {
            tileR.getLeap("PAPER");
        } else if (isScissors) {
            tileR.getLeap("SCISSOR");
        }
    }
}