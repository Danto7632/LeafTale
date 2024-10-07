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

    // Start is called before the first frame update
    void Start()
    {
        power = GetComponent<Image>();

        power.fillAmount = 1;

        maxPower = 60f;
        powerLeft = maxPower;
        powerUsing = 0.003f;

        clawClear = GameObject.Find("box").GetComponent<ClawClear>();
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
            power.fillAmount = 0;
            GameObject.Find("GameManager").GetComponent<GameManager>().AddScore((int)clawClear.clawScore);
            GameObject.Find("GameManager").GetComponent<GameManager>().EndGame(0, 0);
            clawControl.gameOver = true;
        }
    }
}
