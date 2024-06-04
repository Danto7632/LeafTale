using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject enemyPrefab;
    public Vector2[] spawnPosition = new Vector2[3];

    void Start() {
        spawnPosition[0] = new Vector2(-5, 27);
        spawnPosition[1] = new Vector2(0, 27);
        spawnPosition[2] = new Vector2(5, 27);

        for(int i = 0; i < 3; i++) {
            StartCoroutine(spawnEnemies(i));
        }
    }

    IEnumerator spawnEnemies(int num) {
        int randomNumber = Random.Range(0, 3);
        float radomSpawnDelay = Random.Range(5f, 10f);

        Debug.Log(randomNumber);

        Instantiate(enemyPrefab, spawnPosition[num], Quaternion.identity);


        yield return new WaitForSeconds(radomSpawnDelay);

        StartCoroutine(spawnEnemies(num));
    }

}
