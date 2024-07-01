using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapMotionController : MonoBehaviour {
    public GameObject TileCheckerL; // 왼손 타일 체커
    public GameObject TileCheckerR; // 오른손 타일 체커

    private LeapServiceProvider leapProvider;
    private TileCheck tileL;
    private TileCheck tileR;

    public bool isFistL;
    public bool isPalmL;
    public bool isScissorsL;

    public bool isFistR;
    public bool isPalmR;
    public bool isScissorsR;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();

        // 각 타일 체커를 찾아서 연결
        TileCheckerL = GameObject.FindWithTag("tileCheckL");
        TileCheckerR = GameObject.FindWithTag("tileCheckR");

        // 타일 체커의 TileCheck 스크립트 참조
        tileL = TileCheckerL.GetComponent<TileCheck>();
        tileR = TileCheckerR.GetComponent<TileCheck>();

        isFistL = false;
        isFistR = false;

        isPalmL = false;
        isPalmR = false;

        isFistL = false;
        isFistR = false;
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

        if(Input.GetKeyDown(KeyCode.Q)) {
            isFistL = true;
            isPalmL = false;
            isScissorsL = false;
        }
        else if(Input.GetKeyDown(KeyCode.W)) {
            isFistL = false;
            isPalmL = true;
            isScissorsL = false;
        }
        else if(Input.GetKeyDown(KeyCode.E)) {
            isFistL = false;
            isPalmL = false;
            isScissorsL = true;
        }

        if(Input.GetKeyDown(KeyCode.I)) {
            isFistR = true;
            isPalmR = false;
            isScissorsR = false;
        }
        else if(Input.GetKeyDown(KeyCode.O)) {
            isFistR = false;
            isPalmR= true;
            isScissorsR = false;
        }
        else if(Input.GetKeyDown(KeyCode.P)) {
            isFistR = false;
            isPalmR = false;
            isScissorsR = true;
        }

        if (isFistL) {
            tileL.getLeap("ROCK");
        }
        if (isPalmL) {
            tileL.getLeap("PAPER");
        } 
        if (isScissorsL) {
            tileL.getLeap("SCISSOR");
        }

        if (isFistR) {
            tileR.getLeap("ROCK");
        }
        if (isPalmR) {
            tileR.getLeap("PAPER");
        }
        if (isScissorsR) {
            tileR.getLeap("SCISSOR");
        }
    }

    void DetectLeftHandPose(Hand hand) {
        if (hand.GrabStrength > 0.9f) {
            isFistL = true;

            isScissorsL = false;
            isPalmL = false;
        }
        else {
            if(!hand.Fingers[0].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended && 
                hand.Fingers[1].IsExtended && hand.Fingers[2].IsExtended) {
                isScissorsL = true;

                isFistL = false;
                isPalmL = false;
            }
            else if(hand.Fingers[0].IsExtended && hand.Fingers[3].IsExtended && hand.Fingers[4].IsExtended) {
                isPalmL = true;

                isScissorsL = false;
                isFistL = false;
            }
        }

        // 왼손 타일 체커에 동작 전달
        if (isFistL) {
            tileL.getLeap("ROCK");
        }
        if (isPalmL) {
            tileL.getLeap("PAPER");
        } 
        if (isScissorsL) {
            tileL.getLeap("SCISSOR");
        }
    }

    void DetectRightHandPose(Hand hand) {
        if (hand.GrabStrength > 0.9f) {
            isFistR = true;

            isScissorsR = false;
            isPalmR = false;
        }
        else {
            if(!hand.Fingers[0].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended && 
                hand.Fingers[1].IsExtended && hand.Fingers[2].IsExtended) {
                isScissorsR = true;

                isFistR = false;
                isPalmR = false;
            }
            else if(hand.Fingers[0].IsExtended && hand.Fingers[3].IsExtended && hand.Fingers[4].IsExtended) {
                isPalmR = true;

                isScissorsR = false;
                isFistR = false;
            }
        }

        // 오른손 타일 체커에 동작 전달
        if (isFistR) {
            tileR.getLeap("ROCK");
        }
        if (isPalmR) {
            tileR.getLeap("PAPER");
        }
        if (isScissorsR) {
            tileR.getLeap("SCISSOR");
        }
    }
}