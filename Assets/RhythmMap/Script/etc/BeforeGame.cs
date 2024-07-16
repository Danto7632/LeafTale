using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class BeforeGame : MonoBehaviour {
    private LeapServiceProvider leapProvider;

    private GameObject explainPanel;
    public TMP_Text BeforeGameStartText;

    public static bool isGameStart;

    public bool isPointing;
    public float pointingStartTime;
    
    public static float elapsedTime;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        explainPanel = GameObject.Find("ExplainPanel");
        BeforeGameStartText = GameObject.Find("ExplainText").GetComponent<TMP_Text>();

        isGameStart = false;

        elapsedTime = 0f;
    }

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void OnUpdateFrame(Frame frame) {
        if(!isGameStart && frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            if (IsPointingPose(hand)) {
                if (!isPointing) {
                    pointingStartTime = Time.time;

                    isPointing = true;
                }
                else {
                    elapsedTime = Time.time - pointingStartTime;
                    StartBar.ChangeHealthBarAmount(elapsedTime / 3);

                    if (elapsedTime > 3f) {
                        BeforeGameStartText.enabled = false;
                        explainPanel.gameObject.SetActive(false);

                        isGameStart = true;
                        Debug.Log("Game started!");
                    }
                }
            }
            else {
                elapsedTime = 0f;
                isPointing = false;

                StartBar.ChangeHealthBarAmount(elapsedTime);
            }
        }
    }

    void Update() {
        if(!isGameStart) {
            if(Input.GetKeyDown(KeyCode.L)) {
                BeforeGameStartText.enabled = false;
                explainPanel.gameObject.SetActive(false);

                isGameStart = true;
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
}
