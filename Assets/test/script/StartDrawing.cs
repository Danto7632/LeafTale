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

    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        shapeSelector = GameObject.Find("ShapeSelector").GetComponent<ShapeSelector>();
        enemeymanager = GameObject.Find("AnimationManager").GetComponent<Enemeymanager>();

        isDone = false;
    }

    void OnUpdateFrame(Frame frame) {
        if(!isDone) {
            if (frame.Hands.Count > 0) { //사용자가 손을 인식하고 있는지
                hand = frame.Hands[0]; // 인식한 손 중 맨 처음에 인식한 손 하나를 hand변수에 참조

                if (IsPointingPose(hand)) { //인식한 손이 가르키는 손동작을 하고 있는지 확인
                    if (!isPointing) {
                        pointingStartTime = Time.time;

                        isPointing = true;
                    } //특정 손동작을 인식한 시간을 저장
                    else {
                        elapsedTime = Time.time - pointingStartTime;
                        StartBar.ChangeHealthBarAmount(elapsedTime);

                        if (elapsedTime > 3f) { //특정 손동작이 3초 이상 지속되는지 확인 후 게임 실행ㅇ
                            isLeapOn = true;
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
        DrawingGame.isBtnClicked = true;
        isDone = true;
        shapeSelector.gameStart();
        enemeymanager.startEnemey();
        Destroy(gameObject);
    }

    bool IsPointingPose(Hand hand) {
        foreach (Finger finger in hand.Fingers) { //손의 손가락을 모두 가져와 반복문 실행
            if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                if (!finger.IsExtended) return false;
            } //검지가 펴져있지 않다면 false를 반환
            else {
                if (finger.IsExtended) return false;
            } //검지를 제외한 다른 손가락이 펴져있다면 false를 반환
        }
        
        return true; //검지만 펴져있다면 true를 반환
    }
}

