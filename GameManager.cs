using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    public GameObject player;
    public float maxSpawnDelay;
    public float curSpawnDelay;
    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay) {
            SpawnDelay();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }
    }

    void SpawnDelay()
    {
        int randomEnemy = Random.Range(0, 3);
        int randomPoint = Random.Range(0, 9);
        GameObject enemy = Instantiate(enemyObjs[randomEnemy], spawnPoints[randomPoint].position, spawnPoints[randomPoint].rotation);
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        enemyLogic.player = player;

        if (randomPoint == 5 || randomPoint == 6) {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }

        else if (randomPoint == 7 || randomPoint == 8) {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }

        else {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 4;
        player.SetActive(true);
    }
}
