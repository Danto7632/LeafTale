using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSpawn : MonoBehaviour {
    public GameObject timerPrefab;
    public Vector2 spawnPosition;

    public broomMove broomStatus;

    void Start() {
        broomStatus = GameObject.FindWithTag("Player").GetComponent<broomMove>();
    }

    public void StartSpawn() {
        float radomSpawnDelay = Random.Range(0.5f, 2f);
            
        float spawnX = Random.Range(-6f, 6f);

        spawnPosition = new Vector2(spawnX, 70f);
        StartCoroutine(spawnTimer(radomSpawnDelay));
        
    }

    IEnumerator spawnTimer(float startDelay) {
        yield return new WaitForSeconds(startDelay);

        int randomNumber = Random.Range(0, 3);
        float radomSpawnDelay = Random.Range(3f, 7f);

        if(!broomStatus.isGameOver || !broomStatus.isGameClear) {
            Instantiate(timerPrefab, spawnPosition, Quaternion.identity);
        }
        else {
            yield break;
        }

        yield return new WaitForSeconds(radomSpawnDelay);

        StartCoroutine(spawnTimer(startDelay));
    }

}
