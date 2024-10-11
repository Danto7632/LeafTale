using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerBar : MonoBehaviour
{
    Image power;

    public ClawClear clawClear;

    public float maxPower;
    float powerLeft;
    public float powerUsing;

    public GameObject Manager;

    // Start is called before the first frame update
    void Start()
    {
        power = GetComponent<Image>();

        power.fillAmount = 1;

        maxPower = 60f;
        powerLeft = maxPower;
        powerUsing = 0.003f;

        clawClear = GameObject.Find("box").GetComponent<ClawClear>();

        Manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void usePower()
    {
        if (powerLeft > 0)
        {
            powerLeft = Mathf.Clamp(powerLeft, 0.0F, maxPower);
            powerLeft -= powerUsing;
            power.fillAmount = powerLeft / maxPower;
        }
        else
        {
            if(StoryOrStage.instance != null) {
                StoryOrStage.instance.isClawGood = false;
            }
            power.fillAmount = 0;
            Manager.GetComponent<GameManager>().AddScore((int)clawClear.clawScore);
            Manager.GetComponent<GameManager>().EndGame(0, 0);
            clawControl.gameOver = true;
        }
    }
}
