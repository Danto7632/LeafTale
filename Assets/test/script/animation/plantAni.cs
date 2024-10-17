using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantAni : MonoBehaviour {
    public Enemeymanager enemeymanager;

    public bool isIdle;
    public bool isAttack;
    public bool isDie;
    public bool isWalk;

    public bool isExit;

    public Animator anim;
    public playerAni playerani;

    public Vector3 targetPosition;
    public float moveDuration = 2.0f;
    private Vector3 startPosition;
    private float elapsedTime = 0f;

    public magicSoundManager msm;

    public GameObject plantHit;

    public void Start() {
        enemeymanager = transform.parent.GetComponent<Enemeymanager>();
        playerani = GameObject.Find("Player").GetComponent<playerAni>();

        anim = GetComponent<Animator>();

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

    public IEnumerator plantWin() {
        StartCoroutine(playerani.hit2Play(0.6f));

        isIdle = false;
        isAttack = true;

        msm.plantSound.Play();

        yield return new WaitForSeconds(1.4f);

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

        enemeymanager.lastEnemey();

        Destroy(this.gameObject);
    }

    public IEnumerator plantLose() {
        StartCoroutine(playerani.playerWin());

        Instantiate(plantHit, new Vector3(0, 0, 0), Quaternion.identity);
        
        yield return new WaitForSeconds(0.7f);

        isDie = true;
        isIdle = false;

        yield return new WaitForSeconds(1.3f);

        enemeymanager.lastEnemey();

        Destroy(this.gameObject);
    }

    public IEnumerator plantIdle(float delay) {
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
}