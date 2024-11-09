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
    public AudioSource chargedSound;

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
        if (frame.Hands.Count > 0) { //감지된 손이 하나 이상 있는지 확인합니다
            Hand hand = frame.Hands[0]; //감지된 손 중 가장 처음으로 인식된 손을 가져옵니다

            Vector3 currentHandPos = hand.PalmPosition; //손바닥 중앙의 위치를 벡터값으로 저장합니다

            pos = new float[transform.childCount];
            float stageDistance = 1f / (pos.Length - 1f); //현재 스테이지의 개수에 따라서 각 스테이지 간의 위치를 계산합니다

            if (IsPointingPose(hand)) {
                if (!isPointing) {
                    isPointing = true;
                    chargedSound.Play();
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
                chargedSound.Stop();
            } //사용자가 가르키는 손동작을 3초 이상 하고 있다면 지금 스테이지의 게임을 실행합니다

            for (int i = 0; i < pos.Length; i++) {
                pos[i] = stageDistance * i;
            }

            if (IsFist(hand)) { //사용자가 주먹을 쥔 상태인지 확인합니다
                if (!isFisting) { //처음으로 주먹을 쥔 경우 현재 손과 위치를 설정합니다
                    isFisting = true;
                    prevHand = hand;
                    prevHandPos = currentHandPos;
                }
                else { //주먹을 쥔 상태가 계속될 경우 손의 이동거리를 계산하여 특정 거리를 초과 한다면 이동하는 방향에 따라 스크롤 동작을 수행합니다
                    float distance = Vector3.Distance(prevHandPos, currentHandPos);

                    if (distance > swipeDistanceThreshold) { //특정 거리 초과 했는지 검사
                        if (currentHandPos.x > prevHandPos.x) { //오른쪽으로 이동했을 시
                            flipSound.Play();
                            Debug.Log("Right Swipe detected!");
                            scroll_pos = Mathf.Max(scroll_pos - stageDistance, 0f); //스테이지를 스크롤
                        }
                        else if (currentHandPos.x < prevHandPos.x) { //왼쪽으로 이동했을 시
                            flipSound.Play();
                            Debug.Log("Left Swipe detected!");
                            scroll_pos = Mathf.Min(scroll_pos + stageDistance, 1f); //스테이지를 스크롤
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
        return hand.GrabStrength > 0.9f; //손의 쥐기 강도가 0.9f 이상이라면 주먹으로 판단하고 true를 반환
    }

    bool IsPointingPose(Hand hand) {
        foreach (Finger finger in hand.Fingers) { //감지된 손의 손가락을 모두 순회합니다
            if (finger.Type == Finger.FingerType.TYPE_INDEX) { //감지된 손가락 중 검지손가락인지 확인합니다
                if (!finger.IsExtended) return false; //만약 검지 손가락이 펴져있지 않다면 (IsExtended = false) false를 반환합니다
            }
            else {
                if (finger.IsExtended) return false; //검지 이외의 손가락이 펴져있다면 false를 반환합니다
            }
        }
        
        return true; //펴진 손가락이 검지 뿐이라면 true를 반환합니다
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
