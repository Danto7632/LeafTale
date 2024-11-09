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
    private LeapServiceProvider leapProvider; //LeapMotion서 인식하는 손을 참조하기 위함

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
    public bool isGameStart;
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
    private TMP_Text gameEndingText;

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
        gameEndingText = GameObject.Find("GoodEnding_Text").GetComponent<TMP_Text>();
        timer = GameObject.Find("Time");

        bsm = GameObject.Find("SoundManager").GetComponent<broomSoundManager>();

        moveSpeed = 12f;

        isHit = false;
        isGameOver = false;
        isGameStart = false;
        isGameClear = false;

        minX = -6.5f;
        maxX = 6.5f;
        minY = -3.5f;
        maxY = 3f;

        leapProvider = FindObjectOfType<LeapServiceProvider>();

        leapOnText = GameObject.Find("leapOnText").GetComponent<TMP_Text>();
        leapProvider.OnUpdateFrame += OnUpdateFrame; //LeapMotion에서 인식된 손을 참조

        isLeapOn = false;
        isFirstGameStart = false;
        leapOnText.enabled = true;

        plusTimerNum = 0;
        
        if(StoryOrStage.instance.modeFlag == 0)
        {
            gameEndingText.enabled = false;
        }
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

        yield return StartCoroutine(spinPlayer());

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
        float blinkDuration = 0.8f;
        float blinkInterval = 0.1f;
        float blinkTimer = 0.0f;

        StartCoroutine(MoveSmoothly(rb.position, new Vector2(0, -3.5f), blinkDuration));

        while (blinkTimer < blinkDuration) {
            sp.enabled = !sp.enabled;

            yield return new WaitForSeconds(blinkInterval);

            blinkTimer += blinkInterval;
        }

        sp.enabled = true;
    }

    IEnumerator spinPlayer() {
        float spinDuration = 2.0f;
        float spinInterval = 4f;
        float elapsed = 0.0f;

        Vector2 startPosition = rb.position;
        Vector2 targetPosition = new Vector2(rb.position.x, -8);

        while (elapsed < spinDuration) {
            // 회전
            transform.Rotate(0, 0, spinInterval * 360 * Time.deltaTime);
        
            // 이동
            float t = elapsed / spinDuration;
            rb.position = Vector2.Lerp(startPosition, targetPosition, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
        rb.position = targetPosition;
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
        if (frame.Hands.Count > 0) {
            isLeapOn = true;
            hand = frame.Hands[0];

            if(!isGameStart) {
                if (IsPointingPose(hand)) {
                    if (!isPointing) {
                        pointingStartTime = Time.time;

                        isPointing = true;
                        bsm.chargeSound.Play();
                    }
                    else {
                        elapsedTime = Time.time - pointingStartTime;
                        StartBar.ChangeHealthBarAmount(elapsedTime);

                        if (elapsedTime > 3f) {
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
            }

            if(isMoveAllow) {
                DetectHandTilt(hand);
            }
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
        }
    }

    IEnumerator RunGame() {
        isGameStart = true;
        
        yield return new WaitForSeconds(1.0f);

        if (!isFirstGameStart) {
            onTimer.StartCountdown();
            enemySpawn.StartSpawn();
            timerSpawn.StartSpawn();
            isFirstGameStart = true;
        }
    }

    void DetectHandTilt(Hand hand) {
        if (isLeapOn && isMoveAllow && !isHit) { //립모션에 손을 감지하고있는제 게임에서 플레이어가 움직일 수 있는 상태인지 검사합니다
            Vector3 palmNormal = hand.PalmNormal; //손바닥의 수직 벡터를 나타냅니다 손이 평평할때를 기준으로 손이 좌우로 기울어지는 각도를 측정합니다

            if (palmNormal.x > 0.3f || palmNormal.x < -0.3f) { //손바닥이 좌우로 일정 각도를 넘어간다면 플레이어를 좌우로 움직입니다
                horizonLeapSpeed = palmNormal.x;
            }
            else {
                horizonLeapSpeed = 0f;
            }


            if (palmNormal.z > 0.3f || palmNormal.z < -0.3f) { //손바닥이 위아래로 일정 각도를 넘어간다면 플레이어를 위아래로 움직입니다
                verticalLeapSpeed = palmNormal.z;
            }
            else {
                verticalLeapSpeed = 0f;
            }

            moveDirection = new Vector2(horizonLeapSpeed, verticalLeapSpeed);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * -1f, moveDirection.y * moveSpeed * -1f);

            Vector2 clampedPosition = rb.position; //실제로 속도를 적용합니다

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY); //특정 위치 내에서 움직이게 설정합니다

            rb.position = clampedPosition;
        }
        else {
            horizonLeapSpeed = 0f;
            verticalLeapSpeed = 0f;
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

    #endregion
}

