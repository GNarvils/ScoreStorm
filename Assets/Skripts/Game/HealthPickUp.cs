using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour, IInteractable
{
    public int healAmount = 50; 

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>(); 
    }

    public void Interact()
    {
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Debug.Log("Player healed for " + healAmount + " health.");
            Destroy(gameObject);
        }
    }
}
