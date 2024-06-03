using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyBasic; //Parastais pretinieks
    public GameObject enemySpell; //Burvju pretinieks
    public GameObject enemyHeavy; //Stiprais pretinieks
    public Transform player; //Spēlētajs
    public int maxEnemies = 100; //Maksimālais pretinieku daudzums
    public float spawnRadius = 100f; //Parādīšanas rādius
    public int initialSpawnCount = 10; //Cik pretinieki parādās sākumā
    public float minSpawnDistance = 10f; //Minimālais parādīšanas distance

    public int currentEnemyCount = 0; //Cik tagad ir pretinieki uz laukuma
    public int totalEnemyCount = 0; //Cik ir bijuši kopā pretinieki
    public int killedEnemy = 0; //Cik pretinieki ir nogalināti

    public GameTime gameTime;
    private void Awake()
    {
        //Dabū spēlētaju
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);
        GameObject player1Object = GameObject.Find("Player_1");
        GameObject player2Object = GameObject.Find("Player_2");
        if (selectedPlayer == 1)
        {
            if (player1Object != null)
            {
                player = player1Object.transform;
            }
        }
        else if (selectedPlayer == 2)
        {
            if (player2Object != null)
            {
                player = player2Object.transform;
            }
        }

    }
    void Start()
    {
        //Dabū UI objektu, lai dabūtu gameTime skriptu
        GameObject UI = GameObject.Find("UI");
        if (UI != null)
        {
            gameTime = UI.GetComponent<GameTime>();
            if (gameTime == null)
            {
                Debug.LogError("GameTime skripts neatrasts");
            }
        }
        //Parādās sākuma pretinieki
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
        //Ja ir nošauti visi pretinieki spēle beidzās
        if (killedEnemy == maxEnemies)
        {
            gameTime.gameIsOver = true;
        }
        //Pārbauda vai parādītie pretinieku skaits lielāks par maksimālu
        if (totalEnemyCount < maxEnemies)
        {
            //Cik pretinieki vel ir jāparāda
            int enemiesToSpawn = initialSpawnCount - currentEnemyCount;

            if (enemiesToSpawn > 0)
            {
                //Parāda vajadzīgos pretiniekus
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    SpawnEnemy();
                }
            }
        }
    }
    // Parādās pretinieks
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
        //Stiprais pretinieks pārādas tikai, kā 25, 50, 75, 99 un 100 pretinieks
        if (totalEnemyCount == 25 || totalEnemyCount == 50 || totalEnemyCount == 75 || totalEnemyCount == 98 || totalEnemyCount == 99)
        {
            enemyToSpawn = enemyHeavy;
        }
        else
        {
            //Ja neparādās stiprais pretinieks tad ir 50% iespēja, ka parādīsies parastais vai burvju pretinieks.
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

        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
        totalEnemyCount++;

        if (totalEnemyCount >= maxEnemies)
        {
            Debug.Log("Maksimālais pretinieku daudzums sasniekts");
        }
    }
}