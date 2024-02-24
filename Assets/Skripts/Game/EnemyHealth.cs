using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    RagDollManager ragDollManager;
    [HideInInspector] public bool isDead;
    public GameObject head;

    private float damageMultiplier = 1f;
    private Score playerScore;
    private Combo combo;

    private void Start()
    {
        ragDollManager = GetComponent<RagDollManager>();

        playerScore = FindObjectOfType<Score>();

        combo = FindObjectOfType<Combo>();

        // Meklē head objektu 
        if (head == null)
        {
            Transform headTransform = FindChildTransform(transform, "mixamorig:Head");
            if (headTransform != null)
            {
                head = headTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("Head GameObject 'mixamorig:Head' not found.");
            }
        }
    }

    // meklēšanas funkcija
    private Transform FindChildTransform(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }
            Transform result = FindChildTransform(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public void TakeDamage(float damage, bool isHeadshot = false)
    {
        if (health > 0)
        {
            // Pārbauda vai ir head shots
            if (isHeadshot)
            {
                Debug.Log("Headshot!");
            }

            // Pieliek vai reseto damage multiplier ja ir headshot
            if (!isHeadshot)
            {
                damageMultiplier = 1f;
            }
            else
            {
                damageMultiplier = 2f;
            }

            // Damage kalkulācijas
            health -= damage * damageMultiplier;

            if (health <= 0)
            {
                EnemyDeath();
            }
            else
            {
                Debug.Log("Hit enemy");
            }
        }
    }

    // Funkcija, kas notiek, kad nošauj enemy
    void EnemyDeath()
    {
        ragDollManager.TriggerRagdoll();
        Debug.Log("Death enemy");

        //Izsauc combo scriptu
        if (combo != null)
        {
            combo.EnemyKilled();
        }

        // Pievieno Score
        if (playerScore != null)
        {
            playerScore.AddToScore(100);
        }

        StartCoroutine(HideAfterDelay(5f));
    }

    // Paslēpj enemy 5 sekundes pēc nomiršanas
    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}