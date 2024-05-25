﻿using System;
using System.Collections;
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
    public GameObject victoryPanel;
    public GameObject deathT;
    public GameObject timeT;
    public bool gameIsOver = false;
    public GameObject player1Image;
    public GameObject player2Image;
    private bool victoryPanelShown = false;
    public TMP_Text punkti;
    public TMP_Text laiks;
    public TMP_Text kopa;
    public TMP_Text ranks;
    public Score score;
    public int total = 0;
    public AudioClip victorySound; 
    public AudioClip lossSound;   
    public AudioSource audioSource;
    public bool timeIsUp = false;
    public GameObject rekords;
    void Start()
    {
        timeIsUp = false;
        gameIsOver = false;
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);

        if (selectedPlayer == 1)
        {
            player1.SetActive(true);
            player2.SetActive(false);
            playerHealth = player1.GetComponent<PlayerHealth>();
            score = player1.GetComponent<Score>();
        }
        else if (selectedPlayer == 2)
        {
            player1.SetActive(false);
            player2.SetActive(true);
            playerHealth = player2.GetComponent<PlayerHealth>();
            score = player2.GetComponent<Score>();
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
        player1Image = GameObject.Find("1Player");
        player2Image = GameObject.Find("2Player");
        victoryPanel = GameObject.Find("VictoryPanel");
        TMP_Text[] texts = victoryPanel.GetComponentsInChildren<TMP_Text>();

        foreach (TMP_Text text in texts)
        {
            if (text.name == "Punkti")
            {
                punkti = text;
            }
            else if (text.name == "Laiks")
            {
                laiks = text;
            }
            else if (text.name == "Kopa")
            {
                kopa = text;
            }
            else if (text.name == "Ranks")
            {
                ranks = text;
            }
        }
        if (victoryPanel == null)
        {
            Debug.LogError("Panelis nēatrasts");
        }
        else
        {
            victoryPanel.SetActive(false);
        }

        rekords.SetActive(false);
        audioSource = gameObject.GetComponent<AudioSource>(); 
        float volume = PlayerPrefs.GetFloat("Sound", 1.0f);
        audioSource.volume = volume;
    }

    void Update()
    {
        if (!playerHealth.isDead && !gameIsOver) { 
            timer -= Time.deltaTime;
    }

        if (gameIsOver && !victoryPanelShown)
            {
            victoryPanelShown = true;
            ShowVictoryPanel();
            return;
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
            timeIsUp = true;
            Debug.Log("Laiks beidzies!");
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
        PlaySoundEffect(lossSound);
        deadPanel.SetActive(true);
        deathT.SetActive(false);
        timeT.SetActive(true);
    }
    void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);
        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        if (selectedPlayer == 1)
        {
            player1Image.SetActive(true);
            player2Image.SetActive(false);
        }
        else if (selectedPlayer == 2)
        {
            player1Image.SetActive(false);
            player2Image.SetActive(true);
        }
        punkti.text += " " + score.totalScore;
        laiks.text += " " + string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60f), Mathf.FloorToInt(timer % 60f));
        total = (int)(timer + score.totalScore);
        kopa.text += " " + total;
        string rank;
        if (total < 2000)
        {
            rank = "F";
        }
        else if (total < 5000)
        {
            rank = "D";
        }
        else if (total < 10000)
        {
            rank = "C";
        }
        else if (total < 15000)
        {
            rank = "B";
        }
        else if (total < 20000)
        {
            rank = "A";
        }
        else 
        {
            rank = "S";
        }
        ranks.text += " " + rank;

        int previousScore = PlayerPrefs.GetInt("Score_Player_" + selectedPlayer + "_Level_" + selectedLevel, 0);


        if (total > previousScore)
        {
            rekords.SetActive(true);
            PlayerPrefs.SetInt("Score_Player_" + selectedPlayer + "_Level_" + selectedLevel, total);
            PlayerPrefs.Save();
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Uzvara");
        PlaySoundEffect(victorySound);
    }
   public void PlaySoundEffect(AudioClip clip)
    {
            audioSource.PlayOneShot(clip);    
    }
}