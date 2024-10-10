using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemeymanager : MonoBehaviour {
    public wolfAni wolfani;
    public plantAni plantani;
    public dragonAni dragonani;

    public GameObject plantEnemey;
    public GameObject wolfEnemey;
    public GameObject dragonEnemey;

    public bool isWolfStart;
    public bool isPlantStart;
    public bool isDragonStart;

    public bool isTimerOver;
    public bool isPlayerWin;
    public int stageNum;

    public Vector3 targetPosition;
    public float duration;

    private Vector3 startPosition;
    private float elapsedTime = 0f;

    public void Start() {
        wolfEnemey = GameObject.Find("wolf");
        plantEnemey = GameObject.Find("plant");
        dragonEnemey = GameObject.Find("dragon");

        wolfani = wolfEnemey.GetComponent<wolfAni>();
        plantani = plantEnemey.GetComponent<plantAni>();
        dragonani = dragonEnemey.GetComponent<dragonAni>();

        startPosition = wolfEnemey.transform.position;
        targetPosition = new Vector3(4.5f, wolfEnemey.transform.position.y, 0);
        
        isWolfStart = false;
        isPlantStart = false;
        isDragonStart = false;

        duration = 3.498f;

        stageNum = 1;
        isTimerOver = false;
        isPlayerWin = false;
    }


    public void startEnemey() {
        elapsedTime = 0f;
        isWolfStart = true;
        StartCoroutine(wolfani.wolfIdle(3f));
    }

    public void secondEnemey() {
        elapsedTime = 0f;
        isPlantStart = true;
        StartCoroutine(plantani.plantIdle(3f));
    }
    
    public void lastEnemey() {
        elapsedTime = 0f;
        isDragonStart = true;
        StartCoroutine(dragonani.dragonIdle(3f));
    }

    void Update() {
        if (isWolfStart) {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration) {
                targetPosition.y = wolfEnemey.transform.position.y;
                wolfEnemey.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            }
            else {
                wolfEnemey.transform.position = targetPosition;
                isWolfStart = false;
            }
        }

        if (isPlantStart) {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration) {
                targetPosition.y = plantEnemey.transform.position.y;
                plantEnemey.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            }
            else {
                plantEnemey.transform.position = targetPosition;
                isPlantStart = false;
            }
        }

        if (isDragonStart) {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration) {
                targetPosition.y = dragonEnemey.transform.position.y;
                dragonEnemey.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            }
            else {
                dragonEnemey.transform.position = targetPosition;
                isDragonStart = false;
            }
        }

        if(isTimerOver) {
            if(stageNum == 1) {
                StartCoroutine(wolfani.wolfWin());
                isTimerOver = false;
            }
            else if(stageNum == 2) {
                StartCoroutine(plantani.plantWin());
                isTimerOver = false;
            }
            else {
                StartCoroutine(dragonani.dragonWin());
                isTimerOver = false;
            }
            stageNum++;
        }
        else if(isPlayerWin) {
            if(stageNum == 1) {
                StartCoroutine(wolfani.wolfLose());
                isPlayerWin = false;
            }
            else if(stageNum == 2) {
                StartCoroutine(plantani.plantLose());
                isPlayerWin = false;
            }
            else {
                StartCoroutine(dragonani.dragonLose());
                isPlayerWin = false;
            }
            stageNum++;
        }
    }
}
