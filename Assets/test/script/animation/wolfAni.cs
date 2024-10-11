using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfAni : MonoBehaviour {
    public Enemeymanager enemeymanager;

    public playerAni playerani;

    public bool isIdle;
    public bool isAttack;
    public bool isDie;
    public bool isWalk;

    public bool isExit;

    public Animator anim;

    public Vector3 targetPosition;
    public float moveDuration = 2.0f;
    private Vector3 startPosition;
    private float elapsedTime = 0f;

    public magicSoundManager msm;

    public void Start() {
        enemeymanager = transform.parent.GetComponent<Enemeymanager>();
        anim = GetComponent<Animator>();

        playerani = GameObject.Find("Player").GetComponent<playerAni>();

        isIdle = false;
        isAttack = false;
        isDie = false;
        isWalk = false;

        isExit = false;

        startPosition = transform.position;

        msm = GameObject.Find("SoundManager").GetComponent<magicSoundManager>();
    }

    public void Update() {
        animState();

        if (isExit) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1.0f) {
                isWalk = false;
                elapsedTime = 0f;
            }
        }
    }

    public IEnumerator wolfWin() {
        StartCoroutine(playerani.hit1Play());
        isIdle = false;
        isAttack = true;

        StartCoroutine(attackDelay());

        yield return new WaitForSeconds(1f);

        isAttack = false;
        isIdle = true;

        yield return new WaitForSeconds(0.1f);
        
        FlipObject();

        isIdle = false;
        isWalk = true;
        isExit = true;

        targetPosition = new Vector3(transform.position.x + 5.0f, transform.position.y, transform.position.z);
        startPosition = transform.position;

        yield return new WaitForSeconds(moveDuration);

        enemeymanager.secondEnemey();

        Destroy(this.gameObject);
    }

    public IEnumerator wolfLose() {
        StartCoroutine(playerani.playerWin());

        yield return new WaitForSeconds(0.7f);

        isDie = true;
        isIdle = false;

        yield return new WaitForSeconds(1.3f);

        enemeymanager.secondEnemey();

        Destroy(this.gameObject);
    }

    public IEnumerator wolfIdle(float delay) {
        yield return new WaitForSeconds(delay);

        isIdle = true;
        isAttack = false;
        isDie = false;
        isWalk = false;
    }

    void animState() {
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isIdle", isIdle);
        anim.SetBool("isDie", isDie);
        anim.SetBool("isWalk", isWalk);
    }

    public void FlipObject() {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // X 축 스케일을 반전
        transform.localScale = scale;
    }

    public IEnumerator attackDelay() {
        yield return new WaitForSeconds(0.5f);

        msm.wolfSound.Play();
    }
}