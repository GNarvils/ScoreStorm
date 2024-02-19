using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public TMP_Text timeText;
    public float timer = 120f;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            timer = 0f;
        }

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timer <= 0f)
        {
            Debug.Log("Time's up!");
        }
    }

    public void AddTime(float timeToAdd)
    {
        timer += timeToAdd;
    }
}