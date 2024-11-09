using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class BeforeGame : MonoBehaviour {
    private LeapServiceProvider leapProvider;

    private GameObject explainPanel;
    public TMP_Text BeforeGameStartText;
    private TMP_Text gameEndingText;

    public static bool isGameStart;

    public bool isPointing;
    public float pointingStartTime;
    
    public static float elapsedTime;

    public rhythmSoundManager rsm;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        explainPanel = GameObject.Find("ExplainPanel");
        BeforeGameStartText = GameObject.Find("ExplainText").GetComponent<TMP_Text>();
        gameEndingText = GameObject.Find("GoodEnding_Text").GetComponent<TMP_Text>();

        elapsedTime = 0f;
        
        isGameStart = false;

        rsm = GameObject.Find("SoundManager").GetComponent<rhythmSoundManager>();

        if (StoryOrStage.instance.modeFlag == 0) // ���丮����� ���� ����â�� �¿��� ���� ����
        {
            gameEndingText.enabled = false;
        }
    }

    void OnDestroy() {
        if (leapProvider != null) leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame) {
        if(!isGameStart && frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            if (IsPointingPose(hand)) {
                if (!isPointing) {
                    pointingStartTime = Time.time;

                    rsm.chargedSound.Play();
                    isPointing = true;
                }
                else {
                    elapsedTime = Time.time - pointingStartTime;
                    StartBar.ChangeHealthBarAmount(elapsedTime);

                    if (elapsedTime > 3f) {
                        BeforeGameStartText.enabled = false;
                        explainPanel.gameObject.SetActive(false);
                        rsm.chargedSound.Stop();
                        isGameStart = true;
                        rsm.startSound.Play();
                        rsm.rhythmBgm.Play();
                        Debug.Log("Game started!");
                    }
                }
            }
            else {
                elapsedTime = 0f;
                isPointing = false;
                StartBar.ChangeHealthBarAmount(elapsedTime);
            }
        }
    }

    void Update() {
        if(!isGameStart) {
            if(Input.GetKeyDown(KeyCode.L)) {
                BeforeGameStartText.enabled = false;
                explainPanel.gameObject.SetActive(false);

                isGameStart = true;

                rsm.startSound.Play();
                rsm.rhythmBgm.Play();
            }
        }
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
}
