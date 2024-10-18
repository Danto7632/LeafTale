using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class broomSoundManager : MonoBehaviour {

    public AudioSource broomBgm;
    public AudioSource flyingSound;
    public AudioSource itemPickSound;
    public AudioSource FallingSound;

    public AudioSource startSound;

    public AudioSource endSound;

    public AudioSource timerCountSound;

    public AudioSource chargeSound;

    public void start() {
        flyingSound.Play();
    }
}
