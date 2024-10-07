using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAni : MonoBehaviour {
    public bool isIdle;
    public bool isAttack;
    public bool ishit1;
    public bool ishit2;

    public Animator anim;

    public void Start() {
        anim = GetComponent<Animator>();

        isIdle = true;
        isAttack = false;
        ishit1 = false;
        ishit2 = false;
    }

    public void Update() {
        animState();
    }

    public IEnumerator playerWin() {
        isAttack = true;
        isIdle = false;

        yield return new WaitForSeconds(1.9f);

        isAttack = false;
        isIdle = true;
    }

    public IEnumerator hit1Play() {
        yield return new WaitForSeconds(0.35f);

        isIdle = false;
        ishit1 = true;

        yield return new WaitForSeconds(0.5f);

        isIdle = true;
        ishit1 = false;
    }

    public IEnumerator hit2Play(float delay) {
        yield return new WaitForSeconds(delay);
        
        isIdle = false;
        ishit2 = true;

        yield return new WaitForSeconds(1f);

        isIdle = true;
        ishit2 = false;
    }

    void animState() {
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isIdle", isIdle);
        anim.SetBool("ishit1", ishit1);
        anim.SetBool("ishit2", ishit2);
    }
}