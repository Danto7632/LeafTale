using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer_Platform : MonoBehaviour {
    public catMove catStatus;

    public TMP_Text[] timeText;
    public TMP_Text gameOverText;

    public float time;

    public int maxTime;
    public int currentTime;
    public int min;
    public int sec;

    public bool overFlag;
    public bool clearFlag;

    public platSoundManager psm;

    void Awake() {
        catStatus = GameObject.FindWithTag("Player").GetComponent<catMove>();

        time = 120f;
        overFlag = true;
        clearFlag = true;
    }

    void Start() {
        timeText = new TMP_Text[2];

        timeText[0] = GameObject.Find("Min").GetComponent<TMP_Text>();
        timeText[1] = GameObject.Find("Sec").GetComponent<TMP_Text>();
        gameOverText = GameObject.Find("Colon").GetComponent<TMP_Text>();

        psm = GameObject.Find("SoundManager").GetComponent<platSoundManager>();

        timeText[0].text = "02";
        timeText[1].text = "00";
        timeText[0].enabled = false;
        timeText[1].enabled = false;
        gameOverText.enabled = false;

        maxTime = (int)time;
    }

    void Update() {
        if(!catStatus.isGameOver && catStatus.isMoveAllow) {
            time -= Time.deltaTime;

            min = (int)time / 60;
            sec = (int)time % 60;

            if (time <= 0) {
                time = 0;
                if(StoryOrStage.instance != null) {
                    StoryOrStage.instance.isPlatGood = false;
                }
                gameOverText.text = "게임 오버";
                currentTime = (int)time;

                if (overFlag) {
                    GameObject.Find("GameManager").GetComponent<GameManager>().EndGame(currentTime, maxTime);
                    overFlag = false;
                }

                timeText[0].enabled = false;
                timeText[1].enabled = false;
            }   

            if (timeText != null && timeText.Length >= 2) {
                timeText[0].text = min.ToString("00");
                timeText[1].text = sec.ToString("00");
            }
        }
        else if(catStatus.isGameOver) {
            gameOverText.fontSize = 30;
            timeText[0].enabled = false;
            timeText[1].enabled = false;
            gameOverText.text = "게임 클리어";
            
            currentTime = (int)time;

            if (clearFlag) {
                psm.endSound.Play();
                GameObject.Find("GameManager").GetComponent<GameManager>().EndGame(currentTime, maxTime);
                clearFlag = false;
            }
        }
    }

    public void timerStart() {
        StartCoroutine(delayTimer());
    }

    IEnumerator delayTimer() {
        yield return new WaitForSeconds(5f);

        if (timeText != null && timeText.Length >= 2 && gameOverText != null) {
            timeText[0].enabled = true;
            timeText[1].enabled = true;
            gameOverText.enabled = true;
        }
    }
}
