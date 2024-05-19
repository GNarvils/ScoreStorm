using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyBasic; //Parastais pretinieks
    public GameObject enemyHeavy; //Stiprais pretinieks
    public Transform player; // Playeris
    public int maxEnemies = 100; // Limits pretiniekiem
    public float spawnRadius = 100f; // Rādius kur viņi var parādīties
    public int initialSpawnCount = 10; // Sākumā parādās

    public int currentEnemyCount = 0; // tagadējošais pretinieku sakits
    public int totalEnemyCount = 0; //Kopēja pretinieku skaits
    public int killedEnemy = 0; // Nošautie pretinieki

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
        Vector3 spawnPosition = Random.insideUnitSphere * spawnRadius;
        spawnPosition += transform.position;

        spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition);

        if (Vector3.Distance(spawnPosition, player.position) > 50f)
        {
            GameObject enemyToSpawn;

            if (totalEnemyCount >= maxEnemies - 3)
            {
                // Pēdējie trīs pretinieki
                enemyToSpawn = enemyHeavy;
            }
            else if (totalEnemyCount % 10 == 0 && totalEnemyCount != 0)
            {
                // Katrs 10 ir stiprais
                enemyToSpawn = enemyHeavy;
            }
            else
            {
                //Parastie pretinieki
                enemyToSpawn = enemyBasic;
            }

            // Spawn the enemy
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            currentEnemyCount++;
            totalEnemyCount++;
            Debug.Log($"{enemyToSpawn.name} spawned");

            if (totalEnemyCount >= maxEnemies)
            {
                Debug.Log("Maksimālais pretinieku daudzums sasniekts");
            }
        }
    }
}