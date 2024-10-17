using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class clawControl : MonoBehaviour
{
    public LeapServiceProvider leapProvider;

    // Start is called before the first frame update
    public GameObject powerBar;

    public bool clawsOpen;
    public bool goDown, goLeft, goRight;
    Rigidbody2D Rclaw, Lclaw, machine;
    float speed;
    public static bool gameOver;
    public bool isLeapOn;

    public bool isStart;

    public Hand hand;

    public clawSoundManager csm;

    void Awake()
    {
        powerBar = GameObject.Find("powerValue");

        isStart = false;
    }

    void Start()
    {
        leapProvider = FindObjectOfType<LeapServiceProvider>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        Rclaw = GameObject.Find("clawRight").GetComponent<Rigidbody2D>();
        Lclaw = GameObject.Find("clawLeft").GetComponent<Rigidbody2D>();
        machine = gameObject.GetComponent<Rigidbody2D>();

        clawsOpen = true;
        gameOver = false;
        isLeapOn = false;

        csm = GameObject.Find("SoundManager").GetComponent<clawSoundManager>();

        speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver && isStart)
        {
            // Leap Motion이 활성화되었을 때만 Leap Motion을 사용
            if (isLeapOn)
            {
                MoveBasedOnLeapMotion();
            }
            else
            {
                // 키보드 입력을 처리하고, 이동을 수행
                HandleKeyboardInput();
                MoveClaw(); // 키보드에 따른 이동 처리
            }

            // 클로 이동 및 닫힘 처리
            HandleClawMovement();
        }
    }

    // 키보드 입력에 따른 이동 처리 함수 추가
    void MoveClaw()
    {
        // down
        if (goDown && gameObject.transform.position.y > -3.0f && !FireLine.isDownLine)
        {
            gameObject.transform.Translate(0, -speed * Time.deltaTime, 0);
        }
        // left
        if (goLeft && gameObject.transform.position.x > -8.5f && !FireLine.isLeftLine)
        {
            gameObject.transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        // right
        if (goRight && gameObject.transform.position.x < 3.6f && !FireLine.isRightLine)
        {   
            gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);
        }

        if (gameObject.transform.position.y < 3.5f && !goDown)
        {   
            gameObject.transform.Translate(0, speed * Time.deltaTime, 0);
        }
    }

    // 키보드 입력을 처리하는 함수
    void HandleKeyboardInput()
    {
        // down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            goDown = true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            goDown = false;
        }
        // left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            goLeft = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            goLeft = false;
        }
        // right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            goRight = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            goRight = false;
        }
        // claw
        if (Input.GetKeyDown(KeyCode.Space))
        {
            clawsOpen = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            clawsOpen = true;
        }
    }

    // Leap Motion에 따른 이동을 처리하는 함수
    void MoveBasedOnLeapMotion()
    {
        // goLeft, goRight, goDown 값에 따라 이동
        if (!goDown)
        {
            if (gameObject.transform.position.y < 3.5f)
            {
                gameObject.transform.Translate(0, speed * Time.deltaTime, 0);
            }
        }

        if (goDown || goLeft || goRight || !clawsOpen)
        {
            if (goDown && !FireLine.isDownLine)
            {
                // y축의 제한을 키보드와 동일하게 -3.0f로 설정
                if (gameObject.transform.position.y > -3.0f)
                {
                    gameObject.transform.Translate(0, -speed * Time.deltaTime, 0);
                }
            }
            if (goLeft && !FireLine.isLeftLine)
            {
                // x축의 제한을 키보드와 동일하게 -6.32f로 설정
                if (gameObject.transform.position.x > -8.5f)
                {
                    gameObject.transform.Translate(-speed * Time.deltaTime, 0, 0);
                }
            }
            if (goRight && !FireLine.isRightLine)
            {
                // x축의 제한을 키보드와 동일하게 6f로 설정
                if (gameObject.transform.position.x < 3.6f)
                {
                    gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);
                }
            }
        }
    }

    // 클로의 동작을 처리하는 함수
    void HandleClawMovement()
    {
        bool isMoving = false; // 클로가 움직이는지 여부를 저장할 변수

        if (clawsOpen)
        {
            if (Lclaw.transform.eulerAngles.z > 300)
            {
                Lclaw.transform.Rotate(0, 0, -0.5f);
                isMoving = true; // 클로가 움직임
            }
            if (Rclaw.transform.eulerAngles.z < 60)
            {
                Rclaw.transform.Rotate(0, 0, 0.5f);
                isMoving = true; // 클로가 움직임
            }
        }
        else
        {
            if (Lclaw.transform.eulerAngles.z < 353)
            {
                Lclaw.transform.Rotate(0, 0, 0.25f);
                isMoving = true; // 클로가 움직임
            }
            if (Rclaw.transform.eulerAngles.z > 6)
            {
                Rclaw.transform.Rotate(0, 0, -0.25f);
                isMoving = true; // 클로가 움직임
            }
        }   

        // 클로가 위로 움직이는 경우도 추가
        if (gameObject.transform.position.y < 3.5f && !goDown) // 클로가 위로 올라갈 수 있는 최대 Y 위치
        {
            gameObject.transform.Translate(0, speed * Time.deltaTime, 0); // 위로 이동
            isMoving = true; // 클로가 움직임
        }

        // 이동 방향에 따라 powerBar 사용
        if (goLeft || goRight || goDown || (gameObject.transform.position.y < 3.5f))
        {
            powerBar.GetComponent<powerBar>().usePower();
            isMoving = true; // 이동이 발생했으므로 클로가 움직임
        }

        // 소리 재생 또는 정지 처리
        if (isMoving)
        {
            if (!csm.clawSound.isPlaying) // 소리가 재생 중이지 않은 경우
            {
                csm.clawSound.Play(); // 소리 재생
            }
        }
        else
        {
            if (csm.clawSound.isPlaying) // 소리가 재생 중인 경우
            {
                csm.clawSound.Stop(); // 소리 정지
            }
        }
    }
            

    // Leap Motion으로부터 손 데이터를 업데이트하는 함수
    void OnUpdateFrame(Frame frame)
    {
        if (frame.Hands.Count > 0 && !gameOver)
        {
            isLeapOn = true;
            Hand hand = frame.Hands[0]; // 첫 번째 손 선택

            if (IsFist(hand))
            {
                clawsOpen = false;
            }
            else
            {
                clawsOpen = true;
            }

            // 손바닥 기울기에 따른 동작 처리
            if(isLeapOn) {
                DetectHandTilt(hand);
            }
        }
        else
        {
            isLeapOn = false;
        }
    }

    // 손바닥 기울기를 감지하고 이동 방향을 설정하는 함수
    void DetectHandTilt(Hand hand)
    {
        Vector3 palmNormal = hand.PalmNormal; // 손바닥의 기울기를 3D 벡터로 가져옴

        // 좌우 기울기 감지
        if (palmNormal.x > 0.5f)
        {
            goRight = false;
            goLeft = true;
            goDown = false;
        }
        else if (palmNormal.x < -0.5f)
        {
            goLeft = false;
            goRight = true;
            goDown = false;
        }
        else
        {
            goLeft = false;
            goRight = false;
        }

        // 상하 기울기 감지
        if (palmNormal.z < -0.5f) {
            goDown = true;
        }
        else {
            goDown = false;
        }
    }

    // 주먹이 쥐어졌는지 확인하는 함수
    bool IsFist(Hand hand)
    {
        return hand.GrabStrength > 0.9f;
    }
}