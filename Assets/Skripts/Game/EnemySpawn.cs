using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyBasic; //Parastais pretinieks
    public Transform player; // Playeris
    public int maxEnemies = 150; // max limits pretiniekiem
    public float spawnRadius = 100f; // Rādius kur viņi var parādīties
    public int initialSpawnCount = 10; // 10 sākumā parādās

    public int currentEnemyCount = 0; // tagadējošais pretinieku sakits
    public int totalEnemyCount = 0;


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
            Debug.LogError("Neither Player_1 nor Player_2 found in the scene!");
        }


    }
    void Start()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
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
        Vector3 spawnPosition = Random.insideUnitSphere * spawnRadius;
        spawnPosition += transform.position;

        spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition);

        if (Vector3.Distance(spawnPosition, player.position) > 50f) 
        {
            // Spawn the enemy
            Instantiate(enemyBasic, spawnPosition, Quaternion.identity);
            currentEnemyCount++;
            totalEnemyCount++;
            Debug.Log("Enemy spawned");

            if (totalEnemyCount >= maxEnemies)
            {
                Debug.Log("Max enemy count reached.");
            }
        }
    }
}