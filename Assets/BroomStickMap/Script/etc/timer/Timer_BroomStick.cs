using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer_BroomStick : MonoBehaviour {
    public broomMove broomStatus;

    public TMP_Text[] timeText;
    public TMP_Text gameOverText;

    public float time;
    public int min;
    public int sec;

    public bool isStartTimer;

    public broomSoundManager bsm;
    

    void Start() {
        timeText = new TMP_Text[2];

        broomStatus = GameObject.FindWithTag("Player").GetComponent<broomMove>();

        timeText[0] = GameObject.Find("Min").GetComponent<TMP_Text>();
        timeText[1] = GameObject.Find("Sec").GetComponent<TMP_Text>();
        gameOverText = GameObject.Find("Colon").GetComponent<TMP_Text>();

        bsm = GameObject.Find("SoundManager").GetComponent<broomSoundManager>();

        timeText[0].text = "02";
        timeText[1].text = "06";

        timeText[0].enabled = false;
        timeText[1].enabled = false;
        gameOverText.enabled = false;

        time = 37f;
        isStartTimer = true;
    }

    void Update() {
        if(isStartTimer) {
            if(broomStatus.isGameOver) {
                time = 0;
                gameOverText.text = "게임 오버";

                bsm.endSound.Play();
                bsm.flyingSound.Stop();

                timeText[0].enabled = false;
                timeText[1].enabled = false;
            }
            else if(!broomStatus.isGameOver && time <= 0) {
                gameOverText.fontSize = 30;

                timeText[0].enabled = false;
                timeText[1].enabled = false;
                
                broomStatus.isGameClear = true;

                gameOverText.text = "게임 클리어";

                bsm.endSound.Play();
                bsm.flyingSound.Stop();
            }
            else if(!broomStatus.isGameOver && time > 0) {
                time -= Time.deltaTime;

                min = (int)time / 60;
                sec = (int)time % 60;

                timeText[0].text = min.ToString("00");
                timeText[1].text = sec.ToString("00");
            }
        }
    }

    IEnumerator delayTimer() {
        yield return new WaitForSeconds(5f);
    

        if (timeText != null && timeText.Length >= 2 && gameOverText != null) {
            timeText[0].enabled = true;
            timeText[1].enabled = true;
            gameOverText.enabled = true;
        }
    }

    public void StartTimer() {
        Debug.Log("StartTimer");
        StartCoroutine(delayTimer());
    }
}
