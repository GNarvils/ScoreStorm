using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerHealth = 200;
    public bool isDead = false;

    void Update()
    {
        if (!isDead && IsPlayerDead())
        {
            Die();
        }
    }

    // Pārbauda vai spēletājs ir nomiris
     bool IsPlayerDead()
    {
        return playerHealth <= 0;
    }

    // Pārvalda spēlētāja miršanu
    public void Die()
    {
        isDead = true;
        Debug.Log("Player is Dead!");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    // Ļauj lietotājam ņemt damage
    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            playerHealth -= damage;
            Debug.Log("Player took " + damage + " damage. Remaining health: " + playerHealth);
        }
    }
}