using UnityEngine;
using UnityEngine.AI;

public class EnemyHeavyFix : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerHealth playerHealth;
    public AudioSource audioSource;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject player = GameObject.Find("Player_1") ?? GameObject.Find("Player_2");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.OnPlayerDeath += HandlePlayerDeath;
            }
            else
            {
                Debug.LogError("PlayerHealth skirpts neatrasts" + player.name);
            }
        }
        else
        {
            Debug.LogError("Nevar atrast Player_1 vai Player_2 GameObjektu");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource nav atrasts.");
        }
        else {
            audioSource.pitch = 1f;
        }
    }

    void HandlePlayerDeath()
    {
        agent.baseOffset = 0;

    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath -= HandlePlayerDeath;
        }
    }
}