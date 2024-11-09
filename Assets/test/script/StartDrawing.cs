using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class StartDrawing : MonoBehaviour {
    private LeapServiceProvider leapProvider;

    public ShapeSelector shapeSelector;
    public Enemeymanager enemeymanager;

    public Hand hand;
    public bool isLeapOn;

    public bool isPointing;
    public static float elapsedTime;
    public float pointingStartTime;

    public bool isDone;

    public magicSoundManager msm;

    private TMP_Text gameEndingText;

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        shapeSelector = GameObject.Find("ShapeSelector").GetComponent<ShapeSelector>();
        enemeymanager = GameObject.Find("AnimationManager").GetComponent<Enemeymanager>();

        isDone = false;

        msm = GameObject.Find("SoundManager").GetComponent<magicSoundManager>();

        gameEndingText = GameObject.Find("GoodEnding_Text").GetComponent<TMP_Text>();

        if (StoryOrStage.instance.modeFlag == 0) // 스토리모드일 때만 시작창에 굿엔딩 조건 명시
        {
            gameEndingText.enabled = false;
        }
    }

    void OnUpdateFrame(Frame frame) {
        if(!isDone) {
            if (frame.Hands.Count > 0) { //사용자가 손을 인식하고 있는지
                hand = frame.Hands[0]; // 인식한 손 중 맨 처음에 인식한 손 하나를 hand변수에 참조

                if (IsPointingPose(hand)) { //인식한 손이 가르키는 손동작을 하고 있는지 확인
                    if (!isPointing) {
                        pointingStartTime = Time.time;
                        msm.chargedSound.Play();
                        isPointing = true;
                    } //특정 손동작을 인식한 시간을 저장
                    else {
                        elapsedTime = Time.time - pointingStartTime;
                        StartBar.ChangeHealthBarAmount(elapsedTime);

                        if (elapsedTime > 3f) { //특정 손동작이 3초 이상 지속되는지 확인 후 게임 실행ㅇ
                            isLeapOn = true;
                            msm.chargedSound.Stop();
                            RunGame();
                        }
                    }
                }
                else {
                    elapsedTime = 0f;
                    isPointing = false;
                    StartBar.ChangeHealthBarAmount(elapsedTime);
                }
            }

            if (Input.GetKeyDown(KeyCode.L) && !isLeapOn) {
                RunGame();
            } //립모션이 아닌 키보드로 플레이하는 경우 P키를 눌러 시작
        }
    }

    void RunGame() {
        msm.startSound.Play();
        DrawingGame.isBtnClicked = true;
        isDone = true;
        shapeSelector.gameStart();
        enemeymanager.startEnemey();
        Destroy(gameObject);
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

