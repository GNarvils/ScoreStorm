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
    private Rigidbody rb;

    public AudioClip[] enemySounds;
    public AudioSource audioSource;

    public EnemySpawn spawn;
    public int scoreValue = 100;

    private void Start()
    {
        GameObject enemiesObject = GameObject.Find("Enemies");
        if (enemiesObject != null)
        {
            spawn = enemiesObject.GetComponent<EnemySpawn>();
        }
        else
        {
            Debug.LogError("Enemies GameObject nav atrasts!");
        }

        ragdollManager = GetComponent<RagDollManager>();
        playerScore = FindObjectOfType<Score>();
        combo = FindObjectOfType<Combo>();
        gameTime = FindObjectOfType<GameTime>();
        enemyAi = GetComponent<EnemyAi>();
        animator = GetComponent<Animator>();

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.maxDistance = 10f;

        float soundVolume = PlayerPrefs.GetFloat("Sound");
        audioSource.volume = soundVolume;

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
            if (isHeadshot && Random.value >= 0.5f) // 50% iespēja ka ir stagger
            {

                enemyAi.Stagger();
            }

            // Pieliek vai reseto damage multiplier ja ir headshot
            if (!isHeadshot)
            {
                damageMultiplier = 1f;
            }
            else
            {
                Debug.Log("Galvas šaviens!");
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
                Debug.Log("Trāpijis pretiniekam.");
                int rand = Random.Range(6, 8);
                audioSource.PlayOneShot(enemySounds[rand]);
                enemyAi.hasBeenHit = true;
            }
        }
    }

    // Funkcija, kas notiek, kad nošauj enemy
    void EnemyDeath()
    {
        // Trigger ragdoll
        ragdollManager.TriggerRagdoll();
        isDead = true;
        Debug.Log("Nošauts pretinieks");

        int rand = Random.Range(0, 2);
        audioSource.PlayOneShot(enemySounds[rand]);

        //Izsauc combo scriptu
        if (combo != null)
        {
            combo.EnemyKilled();
        }

        // Pievieno Score
        if (playerScore != null)
        {
            playerScore.AddToScore(scoreValue);
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
        StopEnemyMovement();
        StartCoroutine(DestroyAfterDelay(5f));
        spawn.killedEnemy = spawn.killedEnemy + 1;
    }

    // apstādina kustību tā lai neslīdētu pēc nomiršanas
    void StopEnemyMovement()
    {
        if (enemyAi != null && enemyAi.agent != null)
        {
            enemyAi.agent.enabled = false;
        }

        // Disable the Rigidbody to stop physics simulation (if it exists)
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    // Paslēpj enemy 5 sekundes pēc nomiršanas
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        Debug.Log("Pretinieks iznīcināts");
        spawn.currentEnemyCount = spawn.currentEnemyCount - 1;
    }

    public void EnemySpottedSound()
    {
        int rand = Random.Range(2, 4);
        audioSource.PlayOneShot(enemySounds[rand]);
    }

    public void EnemyAttackSound()
    {
        int rand = Random.Range(4, 6);
        audioSource.PlayOneShot(enemySounds[rand]);
    }
}
