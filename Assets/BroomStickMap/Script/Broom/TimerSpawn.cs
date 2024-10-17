using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSpawn : MonoBehaviour {
    public broomMove broomStatus;

    public GameObject timerPrefab;

    public Vector2 spawnPosition;
    private int maxItems;

    void Start() {
        broomStatus = GameObject.FindWithTag("Player").GetComponent<broomMove>();

        maxItems = 0;
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
        float radomSpawnDelay = Random.Range(5f, 19f);

        yield return new WaitForSeconds(radomSpawnDelay);

        if ((!broomStatus.isGameOver || !broomStatus.isGameClear) && maxItems < 3 ) {
            Instantiate(timerPrefab, spawnPosition, Quaternion.identity);
            maxItems++;
        }
        else {
            yield break;
        }

        StartCoroutine(spawnTimer(startDelay));
    }

}
