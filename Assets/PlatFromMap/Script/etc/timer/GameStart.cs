using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class GameStart : MonoBehaviour {
    private LeapServiceProvider leapProvider;
    public GameObject explainPanel;
    public TMP_Text BeforeGameStartText;

    public StartTimer_Platform startTimer_Platform;
    public Timer_Platform timer_Platform;

    public bool isGameStart;

    public bool isPointing;
    public float pointingStartTime;

    public static float elapsedTime;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        explainPanel = GameObject.Find("ExplainPanel");
        BeforeGameStartText = GameObject.Find("ExplainText").GetComponent<TMP_Text>();

        startTimer_Platform = GameObject.Find("StartTimer").GetComponent<StartTimer_Platform>();
        timer_Platform = GameObject.Find("Canvas").GetComponent<Timer_Platform>();

        isGameStart = false;    
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
                        explainPanel.gameObject.SetActive(false);
                        BeforeGameStartText.enabled = false;
                        startTimer_Platform.StartCountdown();
                        timer_Platform.timerStart();
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
            if(Input.GetKeyDown(KeyCode.P)) {
                BeforeGameStartText.enabled = false;
                explainPanel.gameObject.SetActive(false);
                startTimer_Platform.StartCountdown();
                timer_Platform.timerStart();
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
