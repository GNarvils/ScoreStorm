using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class Combo : MonoBehaviour
{

    private int enemiesKilledInCombo;
    private bool isComboActive;
    private float comboTimer;
    public GameObject comboTextObject;
    public TextMeshProUGUI comboCountText;
    public int comboMultiplier;
    public Image comboMeter;
    public float maxComboTime = 10f; //Cik ilgi combo var eksistēt

    void Start()
    {
        ResetCombo();
        UpdateComboUI();
    }

    void Update()
    {
        //Ja combo ir aktīvs tjaunina vajadzīgo ui
        if (isComboActive)
        {
            comboTimer -= Time.deltaTime;
            UpdateComboMeter();
            //Ja combo izbeidzas restartē combo
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
        UpdateComboMeter();
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
        comboMeter.fillAmount = 0;
    }

    // Dabū multiplier no cik enemy ir killed
    public float GetScoreMultiplier()
    {
        if (enemiesKilledInCombo >= 50) return 2f;
        else if (enemiesKilledInCombo >= 40) return 1.8f;
        else if (enemiesKilledInCombo >= 30) return 1.6f;
        else if (enemiesKilledInCombo >= 20) return 1.4f;
        else if (enemiesKilledInCombo >= 10) return 1.2f;
        else return 1f;
    }
    //Atjaunina vizuālo tekstu, gan progresa līniju
    private void UpdateComboUI()
    {
        comboCountText.text = enemiesKilledInCombo.ToString();
    }
    private void UpdateComboMeter()
    {
        comboMeter.fillAmount = comboTimer / maxComboTime;
    }
}
