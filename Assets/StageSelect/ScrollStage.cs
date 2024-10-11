using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class ScrollStage : MonoBehaviour {
    private LeapServiceProvider leapProvider;
    public Hand prevHand;
    public Button tmpButton;

    public GameObject scrollbar;

    public float swipeDistanceThreshold;
    public float pointingStartTime;
    public float scroll_pos;
    public float[] pos;

    public bool isFisting;
    public bool isPointing;

    public Vector3 prevHandPos;

    public static int btnIndex;

    public UnityEngine.UI.Image gaugeImage;

    public AudioSource flipSound;
    public AudioSource selectSound;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        scrollbar = GameObject.Find("Scrollbar Horizontal");

        isFisting = false;

        scroll_pos = 0;
        swipeDistanceThreshold = 3f;

        ResetGauge();
    }

    void Update() {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length  - 1f);

        for (int i = 0; i<pos.Length; i++) {
            pos[i] = distance * i;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            scroll_pos = Mathf.Min(scroll_pos + distance, 1f);
            flipSound.Play();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            scroll_pos = Mathf.Max(scroll_pos - distance, 0f);
            flipSound.Play();
        }

        if (Input.GetMouseButton(0)) {
            selectSound.Play();
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value; 
        }

        else {
            for(int i = 0; i< pos.Length; i++) {
                if(scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }

            for (int i = 0; i<pos.Length; i++) {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                    for(int a = 0; a<pos.Length; a++) {
                        if (a != i) {
                            transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                            tmpButton = transform.GetChild(a).GetComponent<Button>();

                            if(tmpButton.interactable != false) tmpButton.interactable = false;
                        }
                        else {
                            tmpButton = transform.GetChild(a).GetComponent<Button>();

                            if (tmpButton.interactable != true) tmpButton.interactable = true;
                        }

                        btnIndex = i;
                    }
                }
            }
        }
    }

    void OnDestroy() {
        if (leapProvider != null) leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            Vector3 currentHandPos = hand.PalmPosition;

            pos = new float[transform.childCount];
            float stageDistance = 1f / (pos.Length - 1f);

            if (IsPointingPose(hand)) {
                if (!isPointing) {
                    isPointing = true;
                    pointingStartTime = Time.time;
                }
                else if (Time.time - pointingStartTime > 3f) {
                    selectSound.Play();
                    GoStage.SelectLevel();
                    
                }
                UpdateGauge(Time.time - pointingStartTime);
            }
            else {
                ResetGauge();
                isPointing = false;
            }

            for (int i = 0; i < pos.Length; i++) {
                pos[i] = stageDistance * i;
            }

            if (IsFist(hand)) {
                if (!isFisting) {
                    isFisting = true;
                    prevHand = hand;
                    prevHandPos = currentHandPos;
                }
                else {
                    float distance = Vector3.Distance(prevHandPos, currentHandPos);

                    if (distance > swipeDistanceThreshold) {
                        if (currentHandPos.x > prevHandPos.x) {
                            flipSound.Play();
                            Debug.Log("Right Swipe detected!");
                            scroll_pos = Mathf.Max(scroll_pos - stageDistance, 0f);
                        }
                        else if (currentHandPos.x < prevHandPos.x) {
                            flipSound.Play();
                            Debug.Log("Left Swipe detected!");
                            scroll_pos = Mathf.Min(scroll_pos + stageDistance, 1f);
                        }

                        prevHand = hand;
                        prevHandPos = currentHandPos;
                    }
                }
            }
            else {  
                isFisting = false;
            }
        }
    }

    bool IsFist(Hand hand) {
        return hand.GrabStrength > 0.9f;
    } //손의 쥐기 강도를 감지하여 주먹을 쥐었는지 감지하여 true를 반환하는 함수

    bool IsPointingPose(Hand hand) {
        foreach (Finger finger in hand.Fingers) {
            if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                if (!finger.IsExtended) return false;
            }
            else {
                if (finger.IsExtended) return false;
            }
        }
        return true;
    }

    void UpdateGauge(float time) {
        if (gaugeImage != null) {
            gaugeImage.fillAmount = Mathf.Clamp01(time / 3f);
        }
    }

    void ResetGauge() {
        if (gaugeImage != null) {
            gaugeImage.fillAmount = 0f;
        }
    }
}
