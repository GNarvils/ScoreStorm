using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpell : MonoBehaviour
{
    public int damage=50; // Cik daudz dzīvības atņem spēlētājam
    private Transform player; // spēlētājs
    private void Awake()
    {
        //Dabū tagadējošo spēlētāju
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

    //Ja trāpa spēlētājam
    void OnCollisionEnter(Collision collision)
    {
      
        if (collision.transform == player)
        {
            Debug.Log("Collision");
            PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();
            ActionStateManager actions = player.GetComponent<ActionStateManager>();
            if (playerHealth != null && actions != null)
            {
                //Ja spēlētājs bloķē uzbrukumum, tad neatņem dzīvības, bet ja nē tad atņem.
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
        //Iznīcina objektu, kad visas vajadzīgās darbības ir izdarītas.
        Destroy(gameObject);
    }
}
