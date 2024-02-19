using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickUp : MonoBehaviour, IInteractable
{
    public float addedTime = 30f;

    public void Interact()
    {
       
        GameTime gameTime = FindObjectOfType<GameTime>();
        if (gameTime != null)
        {
            gameTime.AddTime(addedTime);
            Debug.Log("Added " + addedTime + " seconds to the game time.");
        }

        gameObject.SetActive(false);
    }
}