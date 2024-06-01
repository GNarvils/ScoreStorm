using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int playerHealth = 200;//Spēlētāja tagadējošās dzīvības
    public int maxPlayerHealth = 200; //Spēlētāja maksimālais iespējamais dzīvības vērtība
    public bool isDead = false;//Vai spēlētājs ir miris
    public Image healthBar;
    public ActionStateManager actions;
    public GameTime time;
    public event Action OnPlayerDeath;

    public AudioClip[] damageSound;
    private AudioSource audioSource;
    private void Start()
    {
        //Dabū vajadžīgos komponentus un vērtības
        time = FindObjectOfType<GameTime>();
        if (time == null)
        {
            Debug.LogError("GameTime skripts nav ainā!");
        }
        audioSource = GetComponent<AudioSource>(); 
        if (audioSource == null)
        {
            Debug.LogError("AudioSource nav atrasts!");
        }

        float soundVolume = PlayerPrefs.GetFloat("Sound");
        audioSource.volume = soundVolume;

    }
    void Update()
    {
        if (!isDead && IsPlayerDead()) //Ja spēlētājam vairs nav dzīvības, tad izpilda miršanas funkciju
        {
            Die();
        }
    }

    // Funkcija, kas pārbauda vai spēlētājs ir miris
     bool IsPlayerDead()
    {
        return playerHealth <= 0;
    }

    // Pārvalda spēlētāja miršanu
    public void Die()
    {
        isDead = true;
        Debug.Log("Spēlētājs ir nomiris!");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; //Padara kursoru redzamu
        actions.SwitchState(actions.Death); //Aiziet uz miršanas stāvokļa
        StartCoroutine(ShowDeadPanel(4f));//Pēc 4 sekundēm parāda miršanas paneli
        OnPlayerDeath?.Invoke();
    }

    // Atņem spēlētājam dzīvinas
    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            playerHealth -= damage;
            if (playerHealth <= 0)
            {
                playerHealth = 0;
                Die();
                audioSource.PlayOneShot(damageSound[1]);
            }
            else {
                audioSource.PlayOneShot(damageSound[0]);
            }
            healthBar.fillAmount = (float)playerHealth / maxPlayerHealth;
            Debug.Log("Spēlētājam atņēma " + damage + " dzīvības. Palikušās dzīvības: " + playerHealth);
            if (actions.currentState != actions.Guard)
            {
                actions.SwitchState(actions.Reaction);
            }
        }
    }
    //Funkcija, kas pārvalda to, ka spēlētājs var dabūt dzīvības atpakaļ
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
            Debug.Log("Spēlētājs dabuja " + healAmount + " dzīvības. Tagadējošās dzīvības: " + playerHealth);
        }
    }

    //Parāda miršanas paneli
    IEnumerator ShowDeadPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        time.deadPanel.SetActive(true);
        time.deathT.SetActive(true);
        time.timeT.SetActive(false);
        time.PlaySoundEffect(time.lossSound);
    }
}