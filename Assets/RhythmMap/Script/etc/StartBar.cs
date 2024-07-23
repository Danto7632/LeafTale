using UnityEngine;
using UnityEngine.UI;

public class StartBar : MonoBehaviour {
    public static Image barImage;

    void Start() {
        barImage = GameObject.Find("StartBar").GetComponent<Image>();
    }

    public static void ChangeHealthBarAmount(float amount) {
        barImage.fillAmount = amount;

        if (barImage.fillAmount == 0f || barImage.fillAmount == 1f) barImage.enabled = false;
        else barImage.enabled = true;
    }
}