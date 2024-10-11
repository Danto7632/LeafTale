
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartTimer_Platform : MonoBehaviour {
    public catMove cat;
    public TMP_Text timerText;

    public int countdownValue;
    public float timer;
    public float interval;
    
    public platSoundManager psm;

    public void Awake() {
        cat = GameObject.FindWithTag("Player").GetComponent<catMove>();
        timerText = GameObject.Find("StartTimer").GetComponent<TMP_Text>();

        psm = GameObject.Find("SoundManager").GetComponent<platSoundManager>();

        countdownValue = 5;
        timer = 0f;
        interval = 1f;

        timerText.enabled = true;
    }

    public void StartCountdown() {
        timer = Time.time;

        StartCoroutine(CountdownCoroutine());
    }

    System.Collections.IEnumerator CountdownCoroutine() {
        while (countdownValue > 0) {
            timerText.text = countdownValue.ToString();

            psm.timerCountSound.Play();
            yield return new WaitForSeconds(interval);

            countdownValue--;
        }

        timerText.text = "GO!";
        cat.isMoveAllow = true;

        yield return new WaitForSeconds(0.5f);
        
        timerText.enabled = false;
    }
}