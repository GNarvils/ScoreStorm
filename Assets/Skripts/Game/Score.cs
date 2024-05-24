using System.Collections;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int totalScore = 0;
    public TextMeshProUGUI scoreText;
    private Combo combo;
    private float scoreMultiplier = 1f;
    private Coroutine multiplierCoroutine;

    void Start()
    {
        combo = FindObjectOfType<Combo>();
        UpdateScoreText();
    }

    public void AddToScore(int value)
    {
        int addedScore = Mathf.RoundToInt(value * combo.GetScoreMultiplier() * scoreMultiplier);
        totalScore += addedScore;
        UpdateScoreText();

        Debug.Log("Pielikts " + addedScore + " pie punktiem.");
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }

    public void ActivateScoreMultiplier(float multiplier, float duration)
    {
        if (multiplierCoroutine != null)
        {
            StopCoroutine(multiplierCoroutine);
        }
        multiplierCoroutine = StartCoroutine(ScoreMultiplierCoroutine(multiplier, duration));
    }

    private IEnumerator ScoreMultiplierCoroutine(float multiplier, float duration)
    {
        scoreMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        scoreMultiplier = 1f;
    }
}
