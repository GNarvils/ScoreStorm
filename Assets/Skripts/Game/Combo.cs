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
    private Coroutine flashingCoroutine;


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
            else if (comboTimer <= 3 && flashingCoroutine == null)
            {
                flashingCoroutine = StartCoroutine(FlashComboText());
            }
        }
    }

    // Coroutine, kas pārvalda tekstu pazušanu/parādīšanos.
    IEnumerator FlashComboText()
    {
        TextMeshProUGUI comboText = comboCountText;
        float duration = 0.5f; 
        float elapsedTime = 0f;
        float targetAlpha = 0f;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float alpha = Mathf.Lerp(0f, 1f, t);
            if (t >= 1f)
            {
                targetAlpha = Mathf.Abs(targetAlpha - 1);
                elapsedTime = 0f;
            }
            comboText.color = new Color(comboText.color.r, comboText.color.g, comboText.color.b, alpha);
            yield return null;
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
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
            flashingCoroutine = null;
        }
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
