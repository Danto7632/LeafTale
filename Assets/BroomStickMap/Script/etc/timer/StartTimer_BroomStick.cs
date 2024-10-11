using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartTimer_BroomStick : MonoBehaviour {
    public TMP_Text timerText;
    public TimerBar_BroomStick timerBar;
    public broomMove broom;

    private int countdownValue;
    private float timer;
    private float interval;

    public broomSoundManager bsm;

    public void Awake() {   
        timerText = GameObject.Find("StartTimer").GetComponent<TMP_Text>();
        timerBar = GameObject.Find("Time").GetComponent<TimerBar_BroomStick>();
        broom =  GameObject.FindWithTag("Player").GetComponent<broomMove>();
        bsm = GameObject.Find("SoundManager").GetComponent<broomSoundManager>();

        timerText.enabled = false;

        countdownValue = 5;
        timer = 0f;
        interval = 1f;
    }

    public void StartCountdown() {
        timer = Time.time;

        timerText.enabled = true;

        StartCoroutine(CountdownCoroutine());
    }

    System.Collections.IEnumerator CountdownCoroutine() {
        while (countdownValue > 0) {
            timerText.text = countdownValue.ToString();

            bsm.timerCountSound.Play();
            
            yield return new WaitForSeconds(interval);

            countdownValue--;
        }

        timerText.text = "GO!";
        bsm.startSound.Play();

        broom.isMoveAllow = true;
        timerBar.StartCoroutine(timerBar.StartTimer());

        yield return new WaitForSeconds(0.5f);
        
        timerText.enabled = false;
    }
}