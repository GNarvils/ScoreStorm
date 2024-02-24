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
        int addedScore = Mathf.RoundToInt(value * combo.GetScoreMultiplier()); 
        totalScore += addedScore; 
        UpdateScoreText();

        Debug.Log("Added " + addedScore + " to score."); 
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
