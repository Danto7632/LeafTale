cat.isFacingRight = true;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    public GameObject player;
    public GameObject readyTimer;
    public Text timerText;

    private int countdownValue = 5;
    private float timer = 0f;
    private float interval = 1f;

    public catMove cat;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player");
        readyTimer = GameObject.Find("Timer");
        cat = player.GetComponent<catMove>();

        timerText = readyTimer.GetComponent<Text>();

        timerText.enabled = true;
    }

    void Start()
    {
        StartCountdown();
    }

    void StartCountdown()
    {
        timer = Time.time;

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
        cat.isMoveAllow = true;

        yield return new WaitForSeconds(0.5f);
        timerText.enabled = false;
    }
}