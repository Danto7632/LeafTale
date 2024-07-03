using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class ScrollStage : MonoBehaviour
{
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;
    private Button tmpButton;

    public static int btnIndex;


    [Header("LeapMotion")]
    private LeapServiceProvider leapProvider;
    public Hand prevHand;
    public Vector3 prevHandPos;
    public float swipeDistanceThreshold;
    public bool isFisting;


    // Start is called before the first frame update
    void Start()
    {   
        scrollbar = GameObject.Find("Scrollbar Horizontal");

        //LeapMotion
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        isFisting = false;

        swipeDistanceThreshold = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length  - 1f);

        for (int i = 0; i<pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        // 방향키 입력 처리
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            scroll_pos = Mathf.Min(scroll_pos + distance, 1f); // 오른쪽 화살표를 누르면 다음 위치로 이동
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            scroll_pos = Mathf.Max(scroll_pos - distance, 0f); // 왼쪽 화살표를 누르면 이전 위치로 이동
        }

        // 마우스 입력 처리
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }

        else
        {
            for(int i = 0; i< pos.Length; i++)
            {
                if(scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }

            for (int i = 0; i<pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                    for(int a = 0; a<pos.Length; a++)
                    {
                        if (a != i)
                        {
                            transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                            tmpButton = transform.GetChild(a).GetComponent<Button>();
                            if(tmpButton.interactable != false)
                            {
                                tmpButton.interactable = false;
                            }
                        }
                        else
                        {
                            tmpButton = transform.GetChild(a).GetComponent<Button>();
                            if (tmpButton.interactable != true)
                            {
                                tmpButton.interactable = true;
                            }
                        }

                        btnIndex = i;
                    }
                }
            }
        }
    }

    void OnDestroy() {
        if (leapProvider != null) {
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }
    }

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            Hand hand = frame.Hands[0];

            Vector3 currentHandPos = hand.PalmPosition;

            pos = new float[transform.childCount];
            float stageDistance = 1f / (pos.Length  - 1f);
        
            for (int i = 0; i<pos.Length; i++)
            {
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
                            Debug.Log("Right Swipe detected!");
                            scroll_pos = Mathf.Max(scroll_pos - stageDistance, 0f);
                        }
                        else if (currentHandPos.x < prevHandPos.x) {
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
    }
}
