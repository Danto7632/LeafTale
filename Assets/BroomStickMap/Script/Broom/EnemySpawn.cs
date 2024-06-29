using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject[] enemyPrefab = new GameObject[3];
    public Vector2[] spawnPosition = new Vector2[3];

    public GameObject Player;
    public broomMove broomStatus;

    void Start() {
        spawnPosition[0] = new Vector2(-6f, 70);
        spawnPosition[1] = new Vector2(0, 70);
        spawnPosition[2] = new Vector2(6f, 70);

        Player = GameObject.FindWithTag("Player");
        broomStatus = Player.GetComponent<broomMove>();
    }

    public void StartSpawn() {
        for(int i = 0; i < 3; i++) {
            float radomSpawnDelay = Random.Range(0.5f, 2f);
            StartCoroutine(spawnEnemies(i, radomSpawnDelay));
        }
    }

    IEnumerator spawnEnemies(int num, float startDelay) {
        yield return new WaitForSeconds(startDelay);

        int randomNumber = Random.Range(0, 3);
        int randomEnemy = Random.Range(0, 3);
        float radomSpawnDelay = Random.Range(3f, 7f);

        if(!broomStatus.isGameOver || !broomStatus.isGameClear) {
            Instantiate(enemyPrefab[randomEnemy], spawnPosition[num], Quaternion.identity);
        }
        else {
            yield break;
        }

        yield return new WaitForSeconds(radomSpawnDelay);

        StartCoroutine(spawnEnemies(num, startDelay));
    }

}
