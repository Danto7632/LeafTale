using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar_BroomStick : MonoBehaviour {
    public Image timerBar;

    public int broom_score;
    public int time_subtract;
    public int time_add;

    public float maxTime;
    public float timeLeft;
    public float timeLast;

    public bool flag;

    void Start() {
        timerBar = GetComponent<Image>();

        timerBar.fillAmount = 1;

        time_subtract = 7;
        time_add = 5;
        maxTime = 60f;
        timeLeft = maxTime;
        timeLast = 0f;

        flag = true;
    }

    public System.Collections.IEnumerator StartTimer() {
        while (flag) {
            if (timeLeft > 0) {
                timeLeft = Mathf.Clamp(timeLeft, 0.0F, maxTime);
                timeLast += Time.deltaTime;
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = timeLeft / maxTime;
            }
            else {
                timerBar.fillAmount = 0;

                GameObject.Find("Player").GetComponent<broomMove>().GameOver();
                
                broom_score = (int)(timeLast * (100 / (maxTime + (time_add * 3))));
                GameObject.Find("GameClear").GetComponent<GameClear>().Clear(broom_score);
                flag = false;
            }
            
            yield return new WaitForSeconds(0.01f);
        }
    }
}
