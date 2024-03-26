using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public TMP_Text timeText;
    public float timer = 120f;
    private PlayerHealth playerHealth; 
    void Start()
    {

        playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found!.");
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
            }
        }
    }

    public void AddTime(float timeToAdd)
    {
        timer += timeToAdd;
    }
}