using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartTimer_BroomStick : MonoBehaviour
{
    public GameObject player;
    public GameObject readyTimer;
    public TMP_Text timerText;

    private int countdownValue = 5;
    private float timer = 0f;
    private float interval = 1f;
    TimerBar_BroomStick timerBar;

    public broomMove broom;

    public void Awake() {
        player = GameObject.FindWithTag("Player");
        readyTimer = GameObject.Find("StartTimer");
        broom = player.GetComponent<broomMove>();

        timerText = readyTimer.GetComponent<TMP_Text>();

        timerText.enabled = true;
        timerBar = GameObject.Find("Time").GetComponent<TimerBar_BroomStick>();
        timerText.enabled = false;
    }

    public void StartCountdown()
    {
        timer = Time.time;

        timerText.enabled = true;

        StartCoroutine(CountdownCoroutine());
    }

    System.Collections.IEnumerator CountdownCoroutine()
    {
        while (countdownValue > 0)
        {
            timerText.text = countdownValue.ToString();

            yield return new WaitForSeconds(interval);

            countdownValue--;
        }

        timerText.text = "GO!";
        broom.isMoveAllow = true;
        timerBar.StartCoroutine(timerBar.StartTimer());
        yield return new WaitForSeconds(0.5f);
        timerText.enabled = false;
    }
}