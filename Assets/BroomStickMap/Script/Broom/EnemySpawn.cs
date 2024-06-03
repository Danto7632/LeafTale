using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject enemyPrefab;
    public Vector2[] spawnPosition = new Vector2[3];
    public float spawnTime;
    public int spawnNumber;

    void Start() {
        spawnTime = 5f;
        spawnNumber = 1;

        spawnPosition[0] = new Vector2(-5, 27);
        spawnPosition[1] = new Vector2(0, 27);
        spawnPosition[2] = new Vector2(5, 27);
        StartCoroutine(spawnEnemies());
    }

    IEnumerator spawnEnemies() {
        yield return new WaitForSeconds(spawnTime);

        int randomNumber = Random.Range(1, 4);

        Debug.Log(randomNumber);


        switch(randomNumber) {
            case 3 :
                for(int i = 0; i < 3; i++) {
                    Instantiate(enemyPrefab, spawnPosition[i], Quaternion.identity);
                }
                break;
            case 2 :
                int randomPosition = Random.Range(0, 3);
                if(randomPosition == 0) {
                    Instantiate(enemyPrefab, spawnPosition[0], Quaternion.identity);
                    Instantiate(enemyPrefab, spawnPosition[2], Quaternion.identity);
                }
                else if(randomPosition == 1) {
                    Instantiate(enemyPrefab, spawnPosition[0], Quaternion.identity);
                    Instantiate(enemyPrefab, spawnPosition[1], Quaternion.identity);
                }
                else {
                    Instantiate(enemyPrefab, spawnPosition[1], Quaternion.identity);
                    Instantiate(enemyPrefab, spawnPosition[2], Quaternion.identity);
                }
                break;
            case 1 :
                int randomPosition_2 = Random.Range(0, 3);
                Instantiate(enemyPrefab, spawnPosition[randomPosition_2], Quaternion.identity);
                break;
        }

        StartCoroutine(spawnEnemies());
    }

}
