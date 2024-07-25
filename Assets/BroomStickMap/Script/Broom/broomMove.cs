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

    [Header("Wrist Movement Tracking")]
    public float totalWristMovement;
    public Text wristMovementText;

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
    public Text leapOnText;

    [Header("Timer")]
    GameObject timer;
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
        wristMovementText = GameObject.Find("WristMovementText").GetComponent<Text>();
        timer = GameObject.Find("Time");

        moveSpeed = 8f;

        isHit = false;
        isGameOver = false;
        isGameClear = false;

        minX = -6.5f;
        maxX = 6.5f;
        minY = -3f;
        maxY = 3f;

        leapProvider = FindObjectOfType<LeapServiceProvider>();

        leapOnText = GameObject.Find("leapOnText").GetComponent<Text>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        isLeapOn = false;
        isFirstGameStart = false;
        leapOnText.enabled = true;

        totalWristMovement = 0f;
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
        }

        if(other.gameObject.CompareTag("PlusTimer") && !isHit) {
            timer.GetComponent<TimerBar_BroomStick>().timeLeft += timer.GetComponent<TimerBar_BroomStick>().time_add;
        }
    }

    IEnumerator hitDelay() {
        timer.GetComponent<TimerBar_BroomStick>().timeLeft -= timer.GetComponent<TimerBar_BroomStick>().time_subtract;
        isHit = true;
        isMoveAllow = false;
        rb.velocity = Vector2.zero;

        yield return StartCoroutine(MoveSmoothly(rb.position, new Vector2(0, -3), 0.3f));

        StartCoroutine(BlinkPlayer());

        yield return new WaitForSeconds(0.7f);

        isMoveAllow = true;

        yield return new WaitForSeconds(0.8f);

        isHit = false;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);
    }

    IEnumerator BlinkPlayer() {
        float blinkDuration = 1.5f;
        float blinkInterval = 0.1f;
        float blinkTimer = 0.0f;

        while (blinkTimer < blinkDuration) {
            sp.enabled = !sp.enabled;

            yield return new WaitForSeconds(blinkInterval);

            blinkTimer += blinkInterval;
        }

        sp.enabled = true;
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
            hand = frame.Hands[0];

            if (IsPointingPose(hand)) {
                if (!isPointing) {
                    pointingStartTime = Time.time;

                    isPointing = true;
                }
                else {
                    elapsedTime = Time.time - pointingStartTime;
                    StartBar.ChangeHealthBarAmount(elapsedTime / 3);

                    if (elapsedTime > 3f) {
                        leapOnText.enabled = false;
                        explainPanel.gameObject.SetActive(false);
                        explainText.enabled = false;

                        StartCoroutine(RunGame());

                        isLeapOn = true;
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
        }

        if (Input.GetKeyDown(KeyCode.P) && !isLeapOn) {
            leapOnText.enabled = false;
            explainPanel.gameObject.SetActive(false);
            explainText.enabled = false;

            StartCoroutine(RunGame());
        }
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
        if (isLeapOn && isMoveAllow && !isHit) {
            Vector3 palmNormal = hand.PalmNormal;

            if (palmNormal.x > 0.3f || palmNormal.x < -0.3f) {
                horizonLeapSpeed = palmNormal.x;
                totalWristMovement += Math.Abs(palmNormal.x) / 100f;
                wristMovementText.text = $"Wrist Movement: {(int)totalWristMovement}";
            }
            else {
                horizonLeapSpeed = 0f;
            }


            if (palmNormal.z > 0.3f || palmNormal.z < -0.3f) {
                verticalLeapSpeed = palmNormal.z;
                totalWristMovement += Math.Abs(palmNormal.z) / 100f;
                wristMovementText.text = $"Wrist Movement: {(int)totalWristMovement}";
            }
            else {
                verticalLeapSpeed = 0f;
            }

            moveDirection = new Vector2(horizonLeapSpeed, verticalLeapSpeed);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * -1f, moveDirection.y * moveSpeed * -1f);

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
        foreach (Finger finger in hand.Fingers) {
            if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                if (!finger.IsExtended) return false;
            }
            else {
                if (finger.IsExtended) return false;
            }
        }
        
        return true;
    }

    #endregion
}

