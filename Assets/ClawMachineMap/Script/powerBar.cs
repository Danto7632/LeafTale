using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class powerBar : MonoBehaviour
{
    public Image power;

    public ClawClear clawClear;

    public clawSoundManager csm;

    public float maxPower;
    float powerLeft;
    public float powerUsing;
    


    // Start is called before the first frame update
    void Start()
    {
        power = GetComponent<Image>();

        if (power == null)
        {
            Debug.LogError("Image component is missing on the GameObject. Please add an Image component.");
            return;
        }

        power.fillAmount = 1;

        maxPower = 60f;
        powerLeft = maxPower;
        powerUsing = 0.003f;

        clawClear = GameObject.Find("trigerArea").GetComponent<ClawClear>();
        csm = GameObject.Find("SoundManager").GetComponent<clawSoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(clawControl.gameOver == true) {
            csm.clawSound.Stop();
        }
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
            GameObject.Find("GameManager").GetComponent<GameManager>().AddScore((int)clawClear.clawScore);
            GameObject.Find("GameManager").GetComponent<GameManager>().EndGame(0, 0);
            clawControl.gameOver = true;
            csm.endSound.Play();
        }
    }
}
