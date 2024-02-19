using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    RagDollManager ragDollManager;
    [HideInInspector] public bool isDead;

    private void Start()
    {
        ragDollManager = GetComponent<RagDollManager>();
    }
    public void TakeDamage(float damage) {
        if (health>0)
        {
            health -= damage;
            if (health <= 0) EnemyDeath();
            else Debug.Log("Hit enemy");
        }
    }

    void EnemyDeath()
    {
        ragDollManager.TriggerRagdoll();
        Debug.Log("Death enemy");

 
        StartCoroutine(HideAfterDelay(5f));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
