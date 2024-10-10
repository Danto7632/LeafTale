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

    public GameObject[] books = new GameObject[6];


    void Start() {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        isFisting = false;

        swipeDistanceThreshold = 3f;

        books = new GameObject[6];

        books[0] = GameObject.Find("ExplainPage");
        books[1] = GameObject.Find("BroomPage");
        books[2] = GameObject.Find("PlatformPage");
        books[3] = GameObject.Find("RhythmPage");
        books[4] = GameObject.Find("MagiccirclePage");
        books[5] = GameObject.Find("ClawPage");

        for(int i = 0; i < 6; i++) {
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
                    books[1].SetActive(true);
                    autoFlip = books[1].transform.Find("Book").GetComponent<AutoFlip>();
                    break;

                case "platformScene" :
                    books[2].SetActive(true);
                    autoFlip = books[2].transform.Find("Book").GetComponent<AutoFlip>();
                    break;

                case "RhythmScene" :
                    books[3].SetActive(true);
                    autoFlip = books[3].transform.Find("Book").GetComponent<AutoFlip>();
                    break;

                case "test" :
                    books[4].SetActive(true);   
                    autoFlip = books[4].transform.Find("Book").GetComponent<AutoFlip>();
                    break;

                case "BroomstickScene" :
                    books[5].SetActive(true);
                    autoFlip = books[5].transform.Find("Book").GetComponent<AutoFlip>();
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
                SceneManager.LoadScene("EndGame");
                break;
        }
    }
}