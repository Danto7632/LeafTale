using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public class LeapMotionFlip : MonoBehaviour {
    private LeapServiceProvider leapProvider;

    public AutoFlip autoFlip;
    Hand prevHand;
    Vector3 prevHandPos;
    float swipeDistanceThreshold;

    bool isFisting;
    bool isPointing;
    float pointingStartTime;


    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        autoFlip = GameObject.Find("Book").GetComponent<AutoFlip>();

        isFisting = false;

        swipeDistanceThreshold = 3f;
    }

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void Update() {
        if(Input.GetKey(KeyCode.LeftArrow)) {
            autoFlip.FlipLeftPage();
        }
        if(Input.GetKey(KeyCode.RightArrow)) {
            autoFlip.FlipRightPage();
        }
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            Vector3 currentHandPos = hand.PalmPosition;

            if (IsPointingPose(hand)) {
                if (!isPointing) {
                    isPointing = true;
                    pointingStartTime = Time.time;
                } else if (Time.time - pointingStartTime > 3f) {
                    SceneManager.LoadScene("StageSelect");
                }
            }
            else {
                isPointing = false;
            }
            
            if(Input.GetKeyDown("p")) {
                SceneManager.LoadScene("StageSelect");
            }

            if (IsFist(hand)) {
                if (!isFisting) {
                    isFisting = true;
                    prevHand = hand;
                    prevHandPos = currentHandPos;
                }
                else {
                    float distance = Vector3.Distance(prevHandPos, currentHandPos);

                    if (distance > swipeDistanceThreshold) {
                        if (currentHandPos.x > prevHandPos.x) {
                            Debug.Log("Right Swipe detected!");
                            autoFlip.FlipLeftPage();
                        }
                        else if (currentHandPos.x < prevHandPos.x) {
                            Debug.Log("Left Swipe detected!");
                            autoFlip.FlipRightPage();
                        }

                        prevHand = hand;
                        prevHandPos = currentHandPos;
                    }
                }
            }
            else {
                isFisting = false;
            }
        }
    }

    bool IsFist(Hand hand) {
        return hand.GrabStrength > 0.9f;
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
}