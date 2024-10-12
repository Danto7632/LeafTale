using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public class LeapMotionFlip : MonoBehaviour {
    private LeapServiceProvider leapProvider;

    public AutoFlip autoFlip;
    Hand prevHand;
    Vector3 prevHandPos;
    float swipeDistanceThreshold;

    bool isFisting;
    bool isPointing;
    float pointingStartTime;

    public GameObject[] books;


    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        isFisting = false;

        swipeDistanceThreshold = 3f;

        books = new GameObject[13];

        books[0] = GameObject.Find("ExplainPage");
        books[1] = GameObject.Find("ClawPage");
        books[2] = GameObject.Find("PlatformPage");
        books[3] = GameObject.Find("RhythmPage");
        books[4] = GameObject.Find("MagiccirclePage_GoodEnd");
        books[5] = GameObject.Find("BroomPage");

        books[6] = GameObject.Find("ClawPage_Fail");
        books[7] = GameObject.Find("PlatformPage_Fail");
        books[8] = GameObject.Find("RhythmPage_Fail");
        books[9] = GameObject.Find("MagiccirclePage_Fail_GoodEnd");
        books[10] = GameObject.Find("BroomPage_Fail");

        books[11] = GameObject.Find("MagiccirclePage_NormalEnd");
        books[12] = GameObject.Find("MagiccirclePage_Fail_NormalEnd");

        for(int i = 0; i < 11; i++) {
            books[i].SetActive(false);
        }


        if(StoryOrStage.instance == null) {
            Debug.Log("NoStoryMode");
        }
        else {
            switch(StoryOrStage.instance.nextStory) {
                case "Explain" :
                    books[0].SetActive(true);
                    autoFlip = books[0].transform.Find("Book").GetComponent<AutoFlip>();
                    break;

                case "ClawMachineScenes" :
                    if(!StoryOrStage.instance.isClawGood) {
                        books[6].SetActive(true);
                        autoFlip = books[6].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    else {
                        books[1].SetActive(true);
                        autoFlip = books[1].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    break;

                case "platformScene" :
                    if(!StoryOrStage.instance.isPlatGood) {
                        books[7].SetActive(true);
                        autoFlip = books[7].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    else {
                        books[2].SetActive(true);
                        autoFlip = books[2].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    break;

                case "RhythmScene" :
                    if(!StoryOrStage.instance.isRhythmGood) {
                        books[8].SetActive(true);
                        autoFlip = books[8].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    else {
                        books[3].SetActive(true);
                        autoFlip = books[3].transform.Find("Book").GetComponent<AutoFlip>();
                    }     
                    break;

                case "test" :
                    if(StoryOrStage.instance.clearCount >= 3 && !StoryOrStage.instance.isMagicGood) {
                        books[9].SetActive(true);
                        autoFlip = books[9].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    else if(StoryOrStage.instance.clearCount >= 3 && StoryOrStage.instance.isMagicGood){
                        books[4].SetActive(true);   
                        autoFlip = books[4].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    else if(StoryOrStage.instance.clearCount < 3 && StoryOrStage.instance.isMagicGood) {
                        books[11].SetActive(true);   
                        autoFlip = books[11].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    else if(StoryOrStage.instance.clearCount < 3 && !StoryOrStage.instance.isMagicGood) {
                        books[12].SetActive(true);   
                        autoFlip = books[12].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    break;

                case "BroomstickScene" :
                    if(!StoryOrStage.instance.isBroomGood) {
                        books[10].SetActive(true);
                        autoFlip = books[10].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    else {
                        books[5].SetActive(true);
                        autoFlip = books[5].transform.Find("Book").GetComponent<AutoFlip>();
                    }
                    break;

            } 
        }
    }

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void Update() {
        if(Input.GetKey(KeyCode.LeftArrow)) {
            autoFlip.FlipLeftPage();
        }
        if(Input.GetKey(KeyCode.RightArrow)) {
            autoFlip.FlipRightPage();
        }
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            Vector3 currentHandPos = hand.PalmPosition;

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
                            Debug.Log("Right Swipe detected!");
                            autoFlip.FlipLeftPage();
                        }
                        else if (currentHandPos.x < prevHandPos.x) {
                            Debug.Log("Left Swipe detected!");
                            autoFlip.FlipRightPage();
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

    public void moveScene() {
        Debug.Log(StoryOrStage.instance.nextStory);
        switch(StoryOrStage.instance.nextStory) {
            case "Explain" :
                SceneManager.LoadScene("ClawMachineScenes");
                break;

            case "ClawMachineScenes" :
                SceneManager.LoadScene("BroomstickScene");
                break;

            case "BroomstickScene" :
                SceneManager.LoadScene("platformScene");
                break;

            case "platformScene" :
                SceneManager.LoadScene("RhythmScene");
                break;

            case "RhythmScene" :
                SceneManager.LoadScene("test");
                break;

            case "test" :
                SceneManager.LoadScene("EndingCredit");
                break;
        }
    }
}