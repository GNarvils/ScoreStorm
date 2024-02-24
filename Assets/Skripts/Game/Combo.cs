using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    private Score playerScore;
    private int enemiesKilledInCombo;
    private bool isComboActive;
    private float comboTimer;
    public GameObject comboTextObject;
    public TextMeshProUGUI comboCountText;
    private int comboMultiplier;

    void Start()
    {
        playerScore = FindObjectOfType<Score>();
        ResetCombo();
        UpdateComboUI();
    }

    void Update()
    {
        if (isComboActive)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    // Kad nogalina enemy activē combo
    public void EnemyKilled()
    {

        enemiesKilledInCombo++;

        comboTimer = 10f;

        if (enemiesKilledInCombo >= 2)
        {
            StartCombo();
        }

        UpdateComboUI();
    }

    //Sāk vai atjaunina combo
    private void StartCombo()
    {
        isComboActive = true;
        comboMultiplier = (int)GetScoreMultiplier();
        comboTextObject.SetActive(true);
    }


    // Restartē combo
    private void ResetCombo()
    {
        isComboActive = false;
        enemiesKilledInCombo = 0;
        comboTextObject.SetActive(false);
    }

    // Dabū multiplier no cik enemy ir killed
    public float GetScoreMultiplier()
    {
        if (enemiesKilledInCombo >= 50) return 1.6f;
        else if (enemiesKilledInCombo >= 40) return 1.5f;
        else if (enemiesKilledInCombo >= 30) return 1.4f;
        else if (enemiesKilledInCombo >= 20) return 1.3f;
        else if (enemiesKilledInCombo >= 10) return 1.2f;
        else if (enemiesKilledInCombo > 2) return 1.1f;
        else return 1f;
    }

    //Atjaunina ui
    private void UpdateComboUI()
    {
        comboCountText.text = enemiesKilledInCombo.ToString();
    }
}
