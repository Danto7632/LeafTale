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
    }

    void Update() {
        if (Input.GetKey(KeyCode.P)) {
            if (!isPressingP) {
                pressingStartTime = Time.time;
                isPressingP = true;
            }
            else {
                elapsedTime = Time.time - pressingStartTime;
                UpdateGauge(elapsedTime);

                if (elapsedTime > 3f && nextSceneName != null) {
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
                }
                else {
                    elapsedTime = Time.time - pointingStartTime;
                    UpdateGauge(elapsedTime);
                    
                    if(elapsedTime > 3f && nextSceneName != null) {
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
        foreach (Finger finger in hand.Fingers) {
            if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                if (!finger.IsExtended) return false;
            }
            else {
                if (finger.IsExtended) return false;
            }
        }
        return true;
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