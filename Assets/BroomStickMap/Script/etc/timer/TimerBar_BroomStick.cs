using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerBar_BroomStick : MonoBehaviour
{
    Image timerBar;
    public float maxTime = 90f;
    public float timeLeft;
    bool flag = true;
    public float timeLast;
    int broom_score; //게임 점수
    GameObject clearObject;

    // Start is called before the first frame update
    void Start()
    {
        clearObject = GameObject.Find("GameClear");
        timerBar = GetComponent<Image> ();
        timeLeft = maxTime;
        timerBar.fillAmount = 1;
        timeLast = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public System.Collections.IEnumerator StartTimer()
    {
        while (flag)
        {
            if (timeLeft > 0)
            {
                timeLast += Time.deltaTime;
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = timeLeft / maxTime;
            }
            else
            {
                timerBar.fillAmount = 0;
                GameObject.Find("Player").GetComponent<broomMove>().GameOver();
                Debug.Log("타임 끝");
                broom_score = (int)(timeLast * (100 / maxTime));
                Debug.Log("Your time is " + broom_score);
                GameObject.Find("GameClear").GetComponent<GameClear>().Clear(broom_score);
                flag = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
