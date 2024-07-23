using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapMotionController : MonoBehaviour {
    public LeapServiceProvider leapProvider;
    public TileCheckL tileL;
    public TileCheckR tileR;

    public bool isFistL;
    public bool isPalmL;
    public bool isScissorsL;

    public bool isFistR;
    public bool isPalmR;
    public bool isScissorsR;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();

        tileL = GameObject.FindWithTag("tileCheckL").GetComponent<TileCheckL>();
        tileR = GameObject.FindWithTag("tileCheckR").GetComponent<TileCheckR>();

        isFistL = false;
        isFistR = false;

        isPalmL = false;
        isPalmR = false;

        isFistL = false;
        isFistR = false;
    }

    void Update() {
        if(BeforeGame.isGameStart) {
            Frame frame = leapProvider.CurrentFrame;

            foreach (Hand hand in frame.Hands) {
                if (hand.IsLeft) DetectLeftHandPose(hand);
                else if (hand.IsRight) DetectRightHandPose(hand);
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

            if (isFistL) tileL.getLeap("ROCK");
            if (isPalmL) tileL.getLeap("PAPER");
            if (isScissorsL) tileL.getLeap("SCISSOR");

            if (isFistR) tileR.getLeap("ROCK");
            if (isPalmR) tileR.getLeap("PAPER");
            if (isScissorsR) tileR.getLeap("SCISSOR");
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

        if (isFistL) tileL.getLeap("ROCK");
        if (isPalmL) tileL.getLeap("PAPER"); 
        if (isScissorsL) tileL.getLeap("SCISSOR");
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

        if (isFistR) tileR.getLeap("ROCK");
        if (isPalmR) tileR.getLeap("PAPER");
        if (isScissorsR) tileR.getLeap("SCISSOR");
    }
}