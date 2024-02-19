using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public TMP_Text time;
    public float timer = 120f;
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            timer = 0f;
        }
        time.text = Mathf.RoundToInt(timer).ToString();

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