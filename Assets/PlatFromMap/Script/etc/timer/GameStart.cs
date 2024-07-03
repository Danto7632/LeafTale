using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class GameStart : MonoBehaviour {
    private LeapServiceProvider leapProvider;
    public TMP_Text BeforeGameStartText;

    public StartTimer_Platform startTimer_Platform;
    public Timer_Platform timer_Platform;

    public bool isGameStart;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        BeforeGameStartText = GameObject.Find("BeforeStart").GetComponent<TMP_Text>();

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
        if(!isGameStart) {
            if (frame.Hands.Count > 1) {
                BeforeGameStartText.enabled = false;
                startTimer_Platform.StartCountdown();
                timer_Platform.timerStart();
                isGameStart = true;
            }
        }
    }

    void Update() {
        if(!isGameStart) {
            if(Input.GetKeyDown(KeyCode.P)) {
                BeforeGameStartText.enabled = false;
                startTimer_Platform.StartCountdown();
                timer_Platform.timerStart();
                isGameStart = true;
            }
        }
    }
}
