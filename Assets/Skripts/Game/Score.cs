using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int totalScore = 0;
    public TextMeshProUGUI scoreText;
    private Combo combo;

    void Start()
    {
        combo = FindObjectOfType<Combo>();
        UpdateScoreText();
    }

    //Funkcija, kas pievieno score
    public void AddToScore(int value)
    {
        totalScore += Mathf.RoundToInt(value * combo.GetScoreMultiplier()); // Reizina score ar combo
        UpdateScoreText();
    }

    // Funkcija, kas updato score vērtību
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }
}
