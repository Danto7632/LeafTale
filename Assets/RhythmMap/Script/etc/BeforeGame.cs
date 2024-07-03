using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class BeforeGame : MonoBehaviour {
    private LeapServiceProvider leapProvider;
    public TMP_Text BeforeGameStartText;

    public static bool isGameStart;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        BeforeGameStartText = GameObject.Find("BeforeStart").GetComponent<TMP_Text>();

        isGameStart = false;    
    }

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void OnUpdateFrame(Frame frame) {
        if(!isGameStart) {
            if (frame.Hands.Count > 1) {
                BeforeGameStartText.enabled = false;

                isGameStart = true;
            }
        }
    }

    void Update() {
        if(!isGameStart) {
            if(Input.GetKeyDown(KeyCode.L)) {
                BeforeGameStartText.enabled = false;

                isGameStart = true;
            }
        }
    }
}
