using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class broomMove : MonoBehaviour {
    [Header("LeapMotion")]
    private LeapServiceProvider leapProvider;

    public bool isLeapOn;
    public bool isFirstGameStart;

    public Hand hand;

    public float horizonLeapSpeed;
    public float verticalLeapSpeed;

    public bool isPointing;
    public float pointingStartTime;

    [Header("Player_Status")]
    public float horiaontalInput;
    public float verticalInput;
    public float moveSpeed;

    [Header("Player_Component")]
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public CapsuleCollider2D capsule2D;
    public Animator anim;
    public StartTimer_BroomStick onTimer;
    public EnemySpawn enemySpawn;
    public TimerSpawn timerSpawn;
    public broomSoundManager bsm;

    [Header("Player_Condition")]
    public bool isHit;
    public bool isMoveAllow;
    public bool isGameOver;
    public bool isGameClear;
    public Vector2 moveDirection;

    [Header("Movement_Limits")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Text")]
    public GameObject explainPanel;
    public TMP_Text explainText;
    public TMP_Text leapOnText;

    [Header("Timer")]
    GameObject timer;
    public int plusTimerNum;
    public static float elapsedTime;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        onTimer = GameObject.Find("StartTimer").GetComponent<StartTimer_BroomStick>();
        enemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        timerSpawn = GameObject.Find("TimerSpawn").GetComponent<TimerSpawn>();
        explainPanel = GameObject.Find("ExplainPanel");
        explainText = GameObject.Find("ExplainText").GetComponent<TMP_Text>();
        timer = GameObject.Find("Time");

        bsm = GameObject.Find("SoundManager").GetComponent<broomSoundManager>();

        moveSpeed = 12f;

        isHit = false;
        isGameOver = false;
        isGameClear = false;

        minX = -6.5f;
        maxX = 6.5f;
        minY = -3.5f;
        maxY = 3f;

        leapProvider = FindObjectOfType<LeapServiceProvider>();

        leapOnText = GameObject.Find("leapOnText").GetComponent<TMP_Text>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        isLeapOn = false;
        isFirstGameStart = false;
        leapOnText.enabled = true;
        
        plusTimerNum = 0;
    }

    void Update() {
        if(!isLeapOn) {
            lineControl();
        }

        if (isGameClear) {
            isMoveAllow = false;
            StartCoroutine(MoveSmoothly(rb.position, new Vector2(0, -3), 0.3f));
        }
    }

    #region GamePlay

    void lineControl() {
        horiaontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (isMoveAllow) {
            moveDirection = new Vector2(horiaontalInput, verticalInput);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }

        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        rb.position = clampedPosition;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("BroomEnemy") && !isHit) {
            StartCoroutine(hitDelay());
            bsm.FallingSound.Play();
        }

        if(other.gameObject.CompareTag("PlusTimer") && !isHit) {
            timer.GetComponent<TimerBar_BroomStick>().timeLeft += timer.GetComponent<TimerBar_BroomStick>().time_add;
            plusTimerNum++;
            bsm.itemPickSound.Play();
        }
    }

    IEnumerator hitDelay() {
        timer.GetComponent<TimerBar_BroomStick>().timeLeft -= timer.GetComponent<TimerBar_BroomStick>().time_subtract;
        isHit = true;
        isMoveAllow = false;
        rb.velocity = Vector2.zero;

        minY = -11f;

        // 2초 동안 회전하면서 (0, -8)로 이동
        yield return StartCoroutine(spinPlayer());

        // 깜빡이면서 (0, -3.5)로 이동
        rb.position = new Vector2(0, -8);

        yield return StartCoroutine(BlinkPlayer());

        minY = -3.5f;
        isMoveAllow = true;
        transform.eulerAngles = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.8f);

        isHit = false;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);
    }

    IEnumerator BlinkPlayer() {
        float blinkDuration = 0.8f; // 깜빡거리는 시간 0.5초로 설정
        float blinkInterval = 0.1f;
        float blinkTimer = 0.0f;

        // MoveSmoothly로 (0, -3.5)로 이동하면서 깜빡거림
        StartCoroutine(MoveSmoothly(rb.position, new Vector2(0, -3.5f), blinkDuration));

        while (blinkTimer < blinkDuration) {
            sp.enabled = !sp.enabled;

            yield return new WaitForSeconds(blinkInterval);

            blinkTimer += blinkInterval;
        }

        sp.enabled = true;
    }

    IEnumerator spinPlayer() {
        float spinDuration = 2.0f; // 2초 동안 회전
        float spinInterval = 4f;
        float elapsed = 0.0f;

        Vector2 startPosition = rb.position; // 현재 위치
        Vector2 targetPosition = new Vector2(rb.position.x, -8); // 최종 목표 위치

        while (elapsed < spinDuration) {
            // 회전
            transform.Rotate(0, 0, spinInterval * 360 * Time.deltaTime);
        
            // 이동
            float t = elapsed / spinDuration;
            rb.position = Vector2.Lerp(startPosition, targetPosition, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
        rb.position = targetPosition; // 최종 위치 설정
    }   

    IEnumerator MoveSmoothly(Vector2 startPosition, Vector2 targetPosition, float duration) {
        float elapsed = 0f;

        while (elapsed < duration) {
            float t = elapsed / duration;
            rb.position = Vector2.Lerp(startPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.position = targetPosition;
    }

    public void GameOver() {
        isGameOver = true;
        isMoveAllow = false;

        StartCoroutine(MoveSmoothly(rb.position, new Vector2(0, -3), 0.3f));

        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0f);
    }

    #endregion


    #region LeapMotion

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) { //사용자가 손을 인식하고 있는지
            isLeapOn = true;
            hand = frame.Hands[0]; // 인식한 손 중 맨 처음에 인식한 손 하나를 hand변수에 참조

            if (IsPointingPose(hand)) { //인식한 손이 가르키는 손동작을 하고 있는지 확인
                if (!isPointing) {
                    pointingStartTime = Time.time;

                    isPointing = true;
                    bsm.chargeSound.Play();
                } //특정 손동작을 인식한 시간을 저장
                else {
                    elapsedTime = Time.time - pointingStartTime;
                    StartBar.ChangeHealthBarAmount(elapsedTime);

                    if (elapsedTime > 3f) { //특정 손동작이 3초 이상 지속되는지 확인 후 게임 실행ㅇ
                        bsm.chargeSound.Stop();
                        leapOnText.enabled = false;
                        explainPanel.gameObject.SetActive(false);
                        explainText.enabled = false;

                        StartCoroutine(RunGame());
                    }
                }
            }
            else {
                elapsedTime = 0f;
                isPointing = false;
                StartBar.ChangeHealthBarAmount(elapsedTime);
            }

            DetectHandTilt(hand);
        }
        else {
            rb.velocity = Vector2.zero;
            isLeapOn = false;
        }

        if (Input.GetKeyDown(KeyCode.L) && !isLeapOn) {
            leapOnText.enabled = false;
            explainPanel.gameObject.SetActive(false);
            explainText.enabled = false;

            StartCoroutine(RunGame());
        } //립모션이 아닌 키보드로 플레이하는 경우 P키를 눌러 시작
    }

    IEnumerator RunGame() {
        yield return new WaitForSeconds(1.0f);

        if (!isFirstGameStart) {
            onTimer.StartCountdown();
            enemySpawn.StartSpawn();
            timerSpawn.StartSpawn();
            isFirstGameStart = true;
        }
    }

    void DetectHandTilt(Hand hand) {
        if (isLeapOn && isMoveAllow && !isHit) { //립모션에 손이 인식되어 있는지 확인, 게임 속 플레이어가 움직일 수 있는 상태인지 확인
            Vector3 palmNormal = hand.PalmNormal; //손바닥이 어느 방향으로 위치해있는지 3D좌표로 저장

            if (palmNormal.x > 0.3f || palmNormal.x < -0.3f) { //손바닥의 좌우 기울기를 확인하여 특정 각도 이상 넘어가면 각도의 값을 저장
                horizonLeapSpeed = palmNormal.x;
            }
            else {
                horizonLeapSpeed = 0f;
            }


            if (palmNormal.z > 0.3f || palmNormal.z < -0.3f) { //손바닥의 상하 기울기를 확인하여 특정 각도 이상 넘어가면 각도의 값을 저장
                verticalLeapSpeed = palmNormal.z;
            }
            else {
                verticalLeapSpeed = 0f;
            }

            moveDirection = new Vector2(horizonLeapSpeed, verticalLeapSpeed);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * -1f, moveDirection.y * moveSpeed * -1f); //기울기에 따라 게임 속 플레이어를 이동

            Vector2 clampedPosition = rb.position;

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

            rb.position = clampedPosition;
        }
        else {
            horizonLeapSpeed = 0f;
            verticalLeapSpeed = 0f;
        }
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

    #endregion
}

