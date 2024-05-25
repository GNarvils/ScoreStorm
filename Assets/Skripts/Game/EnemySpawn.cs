using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyBasic;
    public GameObject enemySpell;
    public GameObject enemyHeavy; 
    public Transform player;
    public int maxEnemies = 100;
    public float spawnRadius = 100f;
    public int initialSpawnCount = 10;
    public float minSpawnDistance = 10f;

    public int currentEnemyCount = 0;
    public int totalEnemyCount = 0;
    public int killedEnemy = 0;

    public GameTime gameTime;
    private void Awake()
    {
        GameObject player1Object = GameObject.Find("Player_1");
        GameObject player2Object = GameObject.Find("Player_2");
      
        if (player1Object != null)
        {
            player = player1Object.transform;
        }
        else if (player2Object != null)
        {
            player = player2Object.transform;
        }
        else
        {
            Debug.LogError("Nav Player_1 vai Player_2");
        }


    }
    void Start()
    {
        GameObject UI = GameObject.Find("UI");
        if (UI != null)
        {
            gameTime = UI.GetComponent<GameTime>();
            if (gameTime == null)
            {
                Debug.LogError("GameTime skripts neatrasts");
            }
        }

        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
        if (killedEnemy == maxEnemies)
        {
            gameTime.gameIsOver = true;
        }

        if (totalEnemyCount < maxEnemies)
        {
            int enemiesToSpawn = initialSpawnCount - currentEnemyCount;

            if (enemiesToSpawn > 0)
            {
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    SpawnEnemy();
                }
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;
        do
        {
            spawnPosition = Random.insideUnitSphere * spawnRadius;
            spawnPosition += transform.position;
            spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition);
        } while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance);

        GameObject enemyToSpawn;

        if (totalEnemyCount == 25 || totalEnemyCount == 50 || totalEnemyCount == 75 || totalEnemyCount == 99 || totalEnemyCount == 100)
        {
            enemyToSpawn = enemyHeavy;
        }
        else
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= 0.5f)
            {
                enemyToSpawn = enemyBasic;
            }
            else
            {
                enemyToSpawn = enemySpell;
            }
        }

        // Spawn the enemy
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
        totalEnemyCount++;

        if (totalEnemyCount >= maxEnemies)
        {
            Debug.Log("Maksimālais pretinieku daudzums sasniekts");
        }
    }
}