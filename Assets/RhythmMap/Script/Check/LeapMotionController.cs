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

    private bool previousFistL, previousPalmL, previousScissorsL;
    private bool previousFistR, previousPalmR, previousScissorsR;

    public rhythmSoundManager rsm;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();

        tileL = GameObject.FindWithTag("tileCheckL").GetComponent<TileCheckL>();
        tileR = GameObject.FindWithTag("tileCheckR").GetComponent<TileCheckR>();

        rsm = GameObject.Find("SoundManager").GetComponent<rhythmSoundManager>();

        isFistL = false;
        isFistR = false;

        isPalmL = false;
        isPalmR = false;

        isFistL = false;
        isFistR = false;

        previousFistL = isFistL;
        previousPalmL = isPalmL;
        previousScissorsL = isScissorsL;

        previousFistR = isFistR;
        previousPalmR = isPalmR;
        previousScissorsR = isScissorsR;
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

        changeSound();
    }

    void DetectLeftHandPose(Hand hand) {
        if (IsFist(hand)) {
            isFistL = true;

            isScissorsL = false;
            isPalmL = false;
        } //주먹 손동작을 하고 있는지 확인
        else {
            if(!hand.Fingers[0].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended && 
                hand.Fingers[1].IsExtended && hand.Fingers[2].IsExtended) { //손가락중 검지, 중지가 펴져있는지 확인
                isScissorsL = true;

                isFistL = false;
                isPalmL = false; 
            } //가위 손동작을 하고 있는지 확인
            else if(hand.Fingers[0].IsExtended && hand.Fingers[1].IsExtended && hand.Fingers[2].IsExtended && 
                hand.Fingers[3].IsExtended && hand.Fingers[4].IsExtended) { //손가락이 전부 펴져있는지 확인
                isPalmL = true;

                isScissorsL = false;
                isFistL = false;
            } //보 손동작을 하고 있는지 확인
        }

        if (isFistL) tileL.getLeap("ROCK");
        if (isPalmL) tileL.getLeap("PAPER"); 
        if (isScissorsL) tileL.getLeap("SCISSOR"); //어느 손동작인지 따라 함수를 실행
    }

    void DetectRightHandPose(Hand hand) {
        if (IsFist(hand)) {
            isFistR = true;

            isScissorsR = false;
            isPalmR = false;
        } //주먹 손동작을 하고 있는지 확인
        else {
            if(!hand.Fingers[0].IsExtended && !hand.Fingers[3].IsExtended && !hand.Fingers[4].IsExtended && 
                hand.Fingers[1].IsExtended && hand.Fingers[2].IsExtended) { //손가락중 검지, 중지가 펴져있는지 확인
                isScissorsR = true;

                isFistR = false;
                isPalmR = false;
            } //가위 손동작을 하고 있는지 확인
            else if(hand.Fingers[0].IsExtended && hand.Fingers[1].IsExtended && hand.Fingers[2].IsExtended && 
                hand.Fingers[3].IsExtended && hand.Fingers[4].IsExtended) { //손가락이 전부 펴져있는지 확인
                isPalmR = true;

                isScissorsR = false;
                isFistR = false;
            } //보 손동작을 하고 있는지 확인
        }

        if (isFistR) tileR.getLeap("ROCK");
        if (isPalmR) tileR.getLeap("PAPER");
        if (isScissorsR) tileR.getLeap("SCISSOR"); //어느 손동작인지 따라 함수를 실행
    }

    bool IsFist(Hand hand) {
        return hand.GrabStrength > 0.9f;
    } //손의 쥐기 강도를 감지하여 주먹을 쥐었는지 감지하여 true를 반환하는 함수


    void changeSound() {
        if (isFistL != previousFistL || isPalmL != previousPalmL || isScissorsL != previousScissorsL) {
            rsm.changeSound.Play();
        }

        if (isFistR != previousFistR || isPalmR != previousPalmR || isScissorsR != previousScissorsR) {
            rsm.changeSound.Play();
        }

        // 현재 손동작 상태를 이전 상태로 저장
        previousFistL = isFistL;
        previousPalmL = isPalmL;
        previousScissorsL = isScissorsL;

        previousFistR = isFistR;
        previousPalmR = isPalmR;
        previousScissorsR = isScissorsR;
    }
}