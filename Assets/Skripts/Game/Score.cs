﻿using System.Collections;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int totalScore = 0; //Cik kopā punktu
    public TextMeshProUGUI scoreText;
    private Combo combo;
    private float scoreMultiplier = 1f; //Punktu reizinātājs
    private Coroutine multiplierCoroutine;

    void Start()
    {
        //Atrod komponentu un atjauno punktu tekstu
        combo = FindObjectOfType<Combo>();
        UpdateScoreText();
    }
    //Metode, kas pieliek klāt punktus
    public void AddToScore(int value)
    {
        int addedScore = Mathf.RoundToInt(value * combo.GetScoreMultiplier() * scoreMultiplier);
        totalScore += addedScore;
        UpdateScoreText();

        Debug.Log("Pielikts " + addedScore + " pie punktiem.");
    }
    //Atjaunina punkta tekstu
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }
    //Sāk punkta reizināšanu
    public void ActivateScoreMultiplier(float multiplier, float duration)
    {
        if (multiplierCoroutine != null)
        {
            StopCoroutine(multiplierCoroutine);
        }
        multiplierCoroutine = StartCoroutine(ScoreMultiplierCoroutine(multiplier, duration));
    }
    //Reizina punktus līdz norādītajam ilgumam
    private IEnumerator ScoreMultiplierCoroutine(float multiplier, float duration)
    {
        scoreMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        scoreMultiplier = 1f;
    }
}
