using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class GameStart : MonoBehaviour {
    public LeapServiceProvider leapProvider;
    public StartTimer_Platform startTimer_Platform;
    public Timer_Platform timer_Platform;

    public GameObject explainPanel;
    public TMP_Text BeforeGameStartText;
    private TMP_Text gameEndingText;

    public float pointingStartTime;

    public bool isGameStart;
    public bool isPointing;

    public static float elapsedTime;

    public platSoundManager psm;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        explainPanel = GameObject.Find("ExplainPanel");

        BeforeGameStartText = GameObject.Find("ExplainText").GetComponent<TMP_Text>();
        gameEndingText = GameObject.Find("GoodEnding_Text").GetComponent<TMP_Text>();
        startTimer_Platform = GameObject.Find("StartTimer").GetComponent<StartTimer_Platform>();
        timer_Platform = GameObject.Find("Canvas").GetComponent<Timer_Platform>();

        psm = GameObject.Find("SoundManager").GetComponent<platSoundManager>();

        isGameStart = false;

        if (StoryOrStage.instance.modeFlag == 0) // 스토리모드일 때만 시작창에 굿엔딩 조건 명시
        {
            gameEndingText.enabled = false;
        }
    }

    void OnDestroy() {
        if (leapProvider != null) leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame) {
        if(!isGameStart && frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            if (IsPointingPose(hand)) {
                if (!isPointing) {
                    pointingStartTime = Time.time;
                    psm.chargedSound.Play();
                    isPointing = true;
                }
                else {
                    elapsedTime = Time.time - pointingStartTime;
                    StartBar.ChangeHealthBarAmount(elapsedTime);

                    if (elapsedTime > 3f) {
                        explainPanel.gameObject.SetActive(false);
                        BeforeGameStartText.enabled = false;
                        startTimer_Platform.StartCountdown();
                        if(timer_Platform == null) {
                            timer_Platform = GameObject.Find("Canvas").GetComponent<Timer_Platform>();
                        }
                        timer_Platform.timerStart();
                        psm.chargedSound.Stop();
                        isGameStart = true;
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
        if(!isGameStart && Input.GetKeyDown(KeyCode.L)) {
            BeforeGameStartText.enabled = false;
            explainPanel.gameObject.SetActive(false);
            startTimer_Platform.StartCountdown();
            if(timer_Platform == null) {
                timer_Platform = GameObject.Find("Canvas").GetComponent<Timer_Platform>();
            }
            timer_Platform.timerStart();
            isGameStart = true;
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
