using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonAni : MonoBehaviour {
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

    public void Start() {
        anim = GetComponent<Animator>();
        playerani = GameObject.Find("Player").GetComponent<playerAni>();

        isIdle = false;
        isAttack = false;
        isDie = false;
        isWalk = false;

        isExit = false;

        startPosition = transform.position;
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

    public IEnumerator dragonWin() {
        StartCoroutine(playerani.hit2Play());
        isIdle = false;
        isAttack = true;

        yield return new WaitForSeconds(2.3f);

        isAttack = false;
        isIdle = true;
    }

    public IEnumerator dragonLose() {
        StartCoroutine(playerani.playerWin());
        Debug.Log("Die dragon");
        isDie = true;
        isIdle = false;

        yield return new WaitForSeconds(2.7f);

        Destroy(this.gameObject);
    }

    public IEnumerator dragonIdle(float delay) {
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
}