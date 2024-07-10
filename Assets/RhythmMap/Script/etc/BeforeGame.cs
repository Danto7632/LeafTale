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

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        explainPanel = GameObject.Find("ExplainPanel");
        BeforeGameStartText = GameObject.Find("ExplainText").GetComponent<TMP_Text>();

        isGameStart = false;    
    }

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void OnUpdateFrame(Frame frame) {
        if(!isGameStart && frame.Hands.Count > 1) {
            Hand hand = frame.Hands[0];
            if(IsPointingPose(hand)) {
                BeforeGameStartText.enabled = false;
                explainPanel.gameObject.SetActive(false);

                isGameStart = true;
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
