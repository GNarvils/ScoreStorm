using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int playerHealth = 200;
    public int maxPlayerHealth = 200;
    public bool isDead = false;
    public Image healthBar;
    public ActionStateManager actions;
    public GameTime time;
    public event Action OnPlayerDeath;
    private void Start()
    {
        time = FindObjectOfType<GameTime>();
        if (time == null)
        {
            Debug.LogError("GameTime skripts nav ainā!");
        }
    }
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
        actions.SwitchState(actions.Death);
        StartCoroutine(ShowDeadPanel(4f));
        OnPlayerDeath?.Invoke();
    }

    // Ļauj lietotājam ņemt damage
    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            playerHealth -= damage;
            if (playerHealth <= 0)
            {
                playerHealth = 0;
                Die();
            }
            healthBar.fillAmount = (float)playerHealth / maxPlayerHealth;
            Debug.Log("Player took " + damage + " damage. Remaining health: " + playerHealth);
            if (actions.currentState != actions.Guard)
            {
                actions.SwitchState(actions.Reaction);
            }
        }
    }
    public void Heal(int healAmount)
    {
        if (!isDead)
        {
            playerHealth += healAmount;
            if (playerHealth > maxPlayerHealth)
            {
                playerHealth = maxPlayerHealth;
            }
            healthBar.fillAmount = (float)playerHealth / maxPlayerHealth;
            Debug.Log("Player healed for " + healAmount + " health. Current health: " + playerHealth);
        }
    }

    IEnumerator ShowDeadPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        time.deadPanel.SetActive(true);
        time.deathT.text = "Jūs esat miruši!";
    }
}