using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public RagDollManager ragdollManager; 
    [HideInInspector] public bool isDead;
    public GameObject head;

    private float damageMultiplier = 1f;
    private Score playerScore;
    private Combo combo;
    private GameTime gameTime;
    private EnemyAi enemyAi;
    private Animator animator;

    private void Start()
    {
        ragdollManager = GetComponent<RagDollManager>();
        playerScore = FindObjectOfType<Score>();
        combo = FindObjectOfType<Combo>();
        gameTime = FindObjectOfType<GameTime>();
        enemyAi = GetComponent<EnemyAi>();
        animator = GetComponent<Animator>();

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
                Debug.LogWarning("Galva nav atrasta!");
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
    
                    enemyAi.Stagger();

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
        // Trigger ragdoll
        ragdollManager.TriggerRagdoll();

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

        // Pievieno laiku
        if (gameTime != null)
        {
            gameTime.AddTime(5f);
        }

        //Izslēdz Ai
        if (enemyAi != null)
        {
            enemyAi.enabled = false;
        }

        // Disable the animator
        if (animator != null)
        {
            animator.enabled = false;
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