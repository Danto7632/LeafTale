using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class broomMove : MonoBehaviour
{
    [Header("LeapMotion")]
    private LeapServiceProvider leapProvider;

    public bool isLeapOn;
    public bool isFirstGameStart;

    public Hand hand;

    public float horizonLeapSpeed;
    public float verticalLeapSpeed;

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
    public Text leapOnText;

    [Header("Timer")]
    GameObject timer;
    public int time_subtract = 3;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        capsule2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        onTimer = GameObject.Find("StartTimer").GetComponent<StartTimer_BroomStick>();
        enemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        timerSpawn = GameObject.Find("TimerSpawn").GetComponent<TimerSpawn>();

        moveSpeed = 8f;

        isHit = false;
        isGameOver = false;
        isGameClear = false;

        minX = -6.5f;
        maxX = 6.5f;
        minY = -3f;
        maxY = 3f;

        //leap
        leapProvider = FindObjectOfType<LeapServiceProvider>();

        leapOnText = GameObject.Find("leapOnText").GetComponent<Text>();
        leapProvider.OnUpdateFrame += OnUpdateFrame;

        isLeapOn = false;
        isFirstGameStart = false;

        leapOnText.enabled = true;

        timer = GameObject.Find("Time");
    }

    void Update()
    {
        lineControl();

        if (isGameClear)
        {
            isMoveAllow = false;
            StartCoroutine(MoveSmoothly(rb.position, new Vector2(0, -3), 0.3f));
        }
    }

    void lineControl()
    {
        horiaontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (!isHit && isMoveAllow)
        {
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
            //hp.GetComponent<HeartUi>().SetHp(-1);
            StartCoroutine(hitDelay());
        }

        if(other.gameObject.CompareTag("PlusTimer") && !isHit) {
            Debug.Log("시간 증가");
        }
    }

    IEnumerator hitDelay()
    {
        timer.GetComponent<TimerBar_BroomStick>().timeLeft -= time_subtract;
        isHit = true;
        rb.velocity = Vector2.zero;

        yield return StartCoroutine(MoveSmoothly(rb.position, new Vector2(0, -3), 0.3f));

        StartCoroutine(BlinkPlayer());

        yield return new WaitForSeconds(1.5f);

        isHit = false;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f);
    }

    IEnumerator BlinkPlayer()
    {
        float blinkDuration = 1.5f;
        float blinkInterval = 0.1f;
        float blinkTimer = 0.0f;

        while (blinkTimer < blinkDuration)
        {
            sp.enabled = !sp.enabled;

            yield return new WaitForSeconds(blinkInterval);

            blinkTimer += blinkInterval;
        }

        sp.enabled = true;
    }

    IEnumerator MoveSmoothly(Vector2 startPosition, Vector2 targetPosition, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rb.position = Vector2.Lerp(startPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.position = targetPosition;
    }

    public void GameOver()
    {
        isGameOver = true;
        isMoveAllow = false;

        StartCoroutine(MoveSmoothly(rb.position, new Vector2(0, -3), 0.3f));

        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0f);
    }


    #region LeapMotion

    void OnUpdateFrame(Frame frame) {
        if (frame.Hands.Count > 0) {
            hand = frame.Hands[0];

            if (IsFist(hand) && !isLeapOn)
            {
                leapOnText.enabled = false;

                StartCoroutine(RunGame());
            }
        }

        else if (Input.GetKeyDown(KeyCode.P)) {
            leapOnText.enabled = false;

            StartCoroutine(RunGame());
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        DetectHandTilt(hand);
    }

    bool IsFist(Hand hand)
    {
        return hand.GrabStrength > 0.9f;
    }

    IEnumerator RunGame() {
        yield return new WaitForSeconds(1.0f);

        if (!isFirstGameStart)
        {
            onTimer.StartCountdown();
            enemySpawn.StartSpawn();
            timerSpawn.StartSpawn();
            isFirstGameStart = true;
        }
        isLeapOn = true;
    }

    void DetectHandTilt(Hand hand)
    {
        if (isLeapOn && isMoveAllow && !isHit)
        {
            Vector3 palmNormal = hand.PalmNormal;

            if (palmNormal.x > 0.2f || palmNormal.x < -0.2f)
            {
                horizonLeapSpeed = palmNormal.x;
            }
            else
            {
                horizonLeapSpeed = 0f;
            }


            if (palmNormal.z > 0.2f || palmNormal.z < -0.2f)
            {
                verticalLeapSpeed = palmNormal.z;
            }
            else
            {
                verticalLeapSpeed = 0f;
            }

            moveDirection = new Vector2(horizonLeapSpeed, verticalLeapSpeed);
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * -1f, moveDirection.y * moveSpeed * -1f);

            Vector2 clampedPosition = rb.position;

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

            rb.position = clampedPosition;
        }
        else
        {
            horizonLeapSpeed = 0f;
            verticalLeapSpeed = 0f;
        }
    }

    #endregion
}