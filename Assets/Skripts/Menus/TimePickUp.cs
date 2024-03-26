using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickUp : MonoBehaviour, IInteractable
{
    public float addedTime = 30f;
    public int scoreToAdd = 25; 

    private Score playerScore;

    private void Start()
    {
        playerScore = FindObjectOfType<Score>(); 
    }

    public void Interact()
    {
        GameTime gameTime = FindObjectOfType<GameTime>();
        if (gameTime != null)
        {
            gameTime.AddTime(addedTime);
            Debug.Log("Added " + addedTime + " seconds to the game time.");
        }

 
        if (playerScore != null)
        {
            playerScore.AddToScore(scoreToAdd);
        }

        Destroy(gameObject);
    }
}