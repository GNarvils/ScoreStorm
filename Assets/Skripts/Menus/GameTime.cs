﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTime : MonoBehaviour
{
    public TMP_Text timeText;
    public float timer = 120f;
    public PlayerHealth playerHealth;
    public GameObject level1;
    public GameObject level2;
    public GameObject player1;
    public GameObject player2;
    public GameObject deadPanel;
    public TMP_Text deathT;
    void Start()
    {
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);

        if (selectedPlayer == 1)
        {
            player1.SetActive(true);
            player2.SetActive(false);
            playerHealth = player1.GetComponent<PlayerHealth>();
        }
        else if (selectedPlayer == 2)
        {
            player1.SetActive(false);
            player2.SetActive(true);
            playerHealth = player2.GetComponent<PlayerHealth>();
        }

        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);

        if (selectedLevel == 1)
        {
            level1.SetActive(true);
            level2.SetActive(false);
        }
        else if (selectedLevel == 2)
        {
            level1.SetActive(false);
            level2.SetActive(true);
        }

        deadPanel = GameObject.Find("DeadPanel");
        if (deadPanel == null)
        {
            Debug.LogError("Panelis nēatrasts");
        }
        else
        {
            deadPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!playerHealth.isDead) { 
            timer -= Time.deltaTime;
    }
        if (timer < 0f)
        {
            timer = 0f;
        }

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timer <= 0f && !playerHealth.isDead)
        {
            Debug.Log("Time's up!");
            if (playerHealth != null)
            {
                playerHealth.Die();
                StartCoroutine(ShowDeadPanel(4f));
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("HomeScreen", LoadSceneMode.Single);
        }
    }

    public void AddTime(float timeToAdd)
    {
        timer += timeToAdd;
    }

    IEnumerator ShowDeadPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        deadPanel.SetActive(true);
        deathT.text = "Laiks ir beidzies!";
    }
}