using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpell : MonoBehaviour
{
    public int damage=50;
    private Transform player;
    private void Awake()
    {
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1); 
        GameObject selectedPlayerObject = null;

        if (selectedPlayer == 1)
        {
            selectedPlayerObject = GameObject.Find("Player1");
        }
        else if (selectedPlayer == 2)
        {
            selectedPlayerObject = GameObject.Find("Player2");
        }
        player = selectedPlayerObject.transform;

    }

    void OnCollisionEnter(Collision collision)
    {
      
        if (collision.transform == player)
        {
            Debug.Log("Collision");
            PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();
            ActionStateManager actions = player.GetComponent<ActionStateManager>();
            if (playerHealth != null && actions != null)
            {
                if (actions.currentState == actions.Guard)
                {
                    Debug.Log("Spēlētājs nobloķēja metienu.");
                }
                else
                {
                    playerHealth.TakeDamage(damage);
                }
            }

        }
        Destroy(gameObject);
    }
}
