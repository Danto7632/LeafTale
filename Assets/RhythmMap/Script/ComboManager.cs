using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    [SerializeField] TMP_Text comboText;
    [SerializeField] TMP_Text comboNum;

    int currentCombo = 0;
    void Start()
    {
        comboText.gameObject.SetActive(false);
        comboNum.gameObject.SetActive(false);
    }

    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        comboNum.text = string.Format("{0:#,##0}", currentCombo);

        if(currentCombo > 0)
        {
            comboText.gameObject.SetActive(true);
            comboNum.gameObject.SetActive(true); 
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        comboNum.text = "0";
        comboText.gameObject.SetActive(false);
        comboNum.gameObject.SetActive(false);
    }

}
