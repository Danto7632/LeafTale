using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUi : MonoBehaviour {
    public Image[] Heart;
    public Sprite Back, Front;

    public int Hp { 
        get; 
        private set; 
    }
    private int Hp_Max;

    public void Awake() {
        Hp_Max = Heart.Length;

        Hp = 5;

        for (int i = 0; i < Hp_Max; i++) {
            if (Hp > i) {
                Heart[i].sprite = Front;
            }
        }
    }

    public void SetHp(int val) {
        Hp += val;

        Hp = Mathf.Clamp(Hp, 0, Hp_Max);

        for (int i = 0; i < Hp_Max; i++) {
            Heart[i].sprite = Back;
        }

        for (int i = 0; i < Hp_Max; i++) {
            if (Hp > i) {
                Heart[i].sprite = Front;
            }
        }
    }
}
