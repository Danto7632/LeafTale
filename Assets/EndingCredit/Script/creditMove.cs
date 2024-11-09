using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class creditMove : MonoBehaviour
{
    public Sprite[] goodEndings;
    GameObject endingPictures;
    int endingCount = 0;
    public float speed = 0.004f;
    public TransitionSettings transition_B;

    private LeapServiceProvider leapProvider;

    public UnityEngine.UI.Image gaugeImage;

    // Start is called before the first frame update
    void Start()
    {
        ResetGauge();
        endingPictures = this.transform.Find("endings").gameObject;

        //change ending pictures
        if (StoryOrStage.instance == null)
        {
            Debug.Log("StoryOrStage does not exist");
        }
        else
        {
            if (StoryOrStage.instance.isClawGood)
            {
                endingPictures.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[0];
                endingCount++;
            }
            if (StoryOrStage.instance.isBroomGood)
            {
                endingPictures.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[1];
                endingCount++;
            }
            if (StoryOrStage.instance.isPlatGood)
            {
                endingPictures.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[2];
                endingCount++;
            }
            if (StoryOrStage.instance.isRhythmGood)
            {
                endingPictures.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[3];
                endingCount++;
            }
            if (StoryOrStage.instance.isMagicGood)
            {
                endingPictures.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[4];
                endingCount++;
            }
            if (endingCount >= 3)
            {
                endingPictures.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[5];
            }
        }

        leapProvider = FindObjectOfType<LeapServiceProvider>();

        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") == true) //skip credit
        {
            goBackToStart();
        }

        //credit move
        if (gameObject.transform.position.y < 45.5f)
        {
            gameObject.transform.Translate(0, speed, 0);
        }
        else
        {
            Invoke("goBackToStart", 2.5f);
        }
    }

    public bool isPointing;
    public float elapsedTime;
    public float pointingStartTime;

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) { //사용자가 손을 인식하고 있는지
            Hand hand = frame.Hands[0]; // 인식한 손 중 맨 처음에 인식한 손 하나를 hand변수에 참조

            if(IsPointingPose(hand)) { //인식한 손이 가르키는 손동작을 하고 있는지 확인
                 if (!isPointing) {
                    pointingStartTime = Time.time;

                    isPointing = true;
                }
                else {
                    elapsedTime = Time.time - pointingStartTime;
                    UpdateGauge(elapsedTime);

                    if (elapsedTime > 3f) {
                        Invoke("goBackToStart", 2.5f);
                    }
                }
            }
            else {
                ResetGauge();
                elapsedTime = 0f;
                isPointing = false;
            }
        }
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

    void goBackToStart()
    {
        TransitionManager.Instance().Transition("StartPage", transition_B, 0);
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
