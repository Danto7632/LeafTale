using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerBar_BroomStick : MonoBehaviour
{
    Image timerBar;
    public float maxTime = 30f;
    public float timeLeft;
    bool flag = true;

    // Start is called before the first frame update
    void Start()
    {
        timerBar = GetComponent<Image> ();
        timeLeft = maxTime;
        timerBar.fillAmount = 1;
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
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = timeLeft / maxTime;
            }
            else
            {
                GameObject.Find("Player").GetComponent<broomMove>().GameOver();
                Debug.Log("≈∏¿” ≥°");
                flag = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
