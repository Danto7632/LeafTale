using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leap;
using Leap.Unity;
using UnityEngine.UI;

public class nextScene : MonoBehaviour {

    private LeapServiceProvider leapProvider;

    public bool isPointing;
    public bool isPressingP;
    public float pointingStartTime;
    public float pressingStartTime;
    public float elapsedTime;

    public UnityEngine.UI.Image gaugeImage;

    public string nextSceneName;

    public LeapMotionFlip leapMotionFlip;

    public flipSoundManager fsm;

    void Start() {
        ResetGauge();

        isPointing = false;
        isPressingP = false;

        pointingStartTime = 0f;
        pressingStartTime = 0f;
        elapsedTime = 0f;

        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        leapMotionFlip = GameObject.Find("LeapMotionManager").GetComponent<LeapMotionFlip>();

        fsm = GameObject.Find("SoundManager").GetComponent<flipSoundManager>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.P)) {
            if (!isPressingP) {
                pressingStartTime = Time.time;
                fsm.chargedSound.Play();
                isPressingP = true;
            }
            else {
                elapsedTime = Time.time - pressingStartTime;
                UpdateGauge(elapsedTime);

                if (elapsedTime > 3f && nextSceneName != null) {
                    fsm.selectSound.Play();
                    fsm.chargedSound.Stop();
                    leapMotionFlip.moveScene();
                }
            }
        }
        else {

            if (isPressingP) {
                ResetGauge();
                isPressingP = false;
            }
        }
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            if (IsPointingPose(hand)) {
                if (!isPointing) {
                    pointingStartTime = Time.time;
                    isPointing = true;
                    fsm.chargedSound.Play();
                }
                else {
                    elapsedTime = Time.time - pointingStartTime;
                    UpdateGauge(elapsedTime);
                    
                    if(elapsedTime > 3f && nextSceneName != null) {
                        fsm.selectSound.Play();
                        fsm.chargedSound.Stop();
                        leapMotionFlip.moveScene();
                    }
                }
            }
            else {
                ResetGauge();
                elapsedTime = 0f;
                isPointing = false;
            }
        }
    }

    bool IsPointingPose(Hand hand) {
        foreach (Finger finger in hand.Fingers) { //감지된 손의 손가락을 모두 순회합니다
            if (finger.Type == Finger.FingerType.TYPE_INDEX) { //감지된 손가락 중 검지손가락인지 확인합니다
                if (!finger.IsExtended) return false; //만약 검지 손가락이 펴져있지 않다면 (IsExtended = false) false를 반환합니다
            }
            else {
                if (finger.IsExtended) return false; //검지 이외의 손가락이 펴져있다면 false를 반환합니다
            }
        }
        
        return true; //펴진 손가락이 검지 뿐이라면 true를 반환합니다
    }

    void UpdateGauge(float time) {
        if (gaugeImage != null) {
            gaugeImage.fillAmount = Mathf.Clamp01(time / 3f);
        }
    }

    void ResetGauge() {
        if (gaugeImage != null) {
            gaugeImage.fillAmount = 0f;
        }
    }
}