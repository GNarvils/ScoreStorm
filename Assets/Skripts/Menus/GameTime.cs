using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTime : MonoBehaviour
{
    public TMP_Text timeText; 
    public float timer = 120f; // Sākotnējais taimeris vērtība
    public PlayerHealth playerHealth;
    public GameObject level1; 
    public GameObject level2;
    public GameObject player1; 
    public GameObject player2;
    public GameObject deadPanel;
    public GameObject victoryPanel; 
    public GameObject deathT; 
    public GameObject timeT; 
    public bool gameIsOver = false; // Vai spēle ir beigusies
    public GameObject player1Image; 
    public GameObject player2Image;
    private bool victoryPanelShown = false; // Vai uzvaras panelis jau ir parādīts
    public TMP_Text punkti; 
    public TMP_Text laiks; 
    public TMP_Text kopa;
    public TMP_Text ranks; 
    public Score score; 
    public int total = 0; // Kopējais punktu skaits
    public AudioClip victorySound;
    public AudioClip lossSound; 
    public AudioSource audioSource; 
    public bool timeIsUp = false; 
    public GameObject rekords;
    public GameObject multiplierT;

    void Start()
    {
        timeIsUp = false;
        gameIsOver = false;
        multiplierT.SetActive(false);
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);

        // Iestata spēlētāju un punktu sistēmu, atkarībā no izvēlētā spēlētāja
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

        // Iestata līmeni, atkarībā no izvēlētā līmeņa
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

        // Atrod uzvaras paneļa tekstus
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
        // Atjauno taimeri tikai, ja spēlētājs nav miris un spēle nav beigusies
        if (!playerHealth.isDead && !gameIsOver)
        {
            timer -= Time.deltaTime;
        }

        // Ja spēle ir beigusies un uzvaras panelis vēl nav rādīts, rādīt uzvaras paneli
        if (gameIsOver && !victoryPanelShown)
        {
            victoryPanelShown = true;
            ShowVictoryPanel();
            return;
        }

        // Nodrošina, ka taimeris nekad nav mazāks par 0
        if (timer < 0f)
        {
            timer = 0f;
        }

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Ja laiks ir beidzies un spēlētājs nav miris, spēlētājs nomirst
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

        // Izeja no spēles uz mājas ekrānu ar Esc taustiņu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("HomeScreen", LoadSceneMode.Single);
        }
    }

    // Metode, lai pievienotu laiku taimerim
    public void AddTime(float timeToAdd)
    {
        timer += timeToAdd;
    }

    // Korutīna, kas parāda nāves paneli pēc aizkaves
    IEnumerator ShowDeadPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySoundEffect(lossSound);
        deadPanel.SetActive(true);
        deathT.SetActive(false);
        timeT.SetActive(true);
    }

    // Metode, kas parāda uzvaras paneli
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

        // Noteikta ranga piešķiršana, balstoties uz kopējo punktu skaitu
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

        // Pārbauda un saglabā jauno rekordu, ja tas ir lielāks par iepriekšējo
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

    // Metode, lai atskaņotu skaņas efektu
    public void PlaySoundEffect(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
