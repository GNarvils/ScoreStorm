using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy;
    [HideInInspector] public WeaponManager weapon;
    [HideInInspector] public Vector3 dir;

    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();

            
            GameObject head = enemyHealth.head;

            // CPārbauda vai ir headshots
            bool isHeadshot = IsHeadshot(collision.contacts[0].point, head);

            enemyHealth.TakeDamage(weapon.damage, isHeadshot);

            if (enemyHealth.health <= 0 && enemyHealth.isDead == false)
            {
                Rigidbody rb = collision.gameObject.GetComponentInChildren<Rigidbody>();
                rb.AddForce(dir * weapon.enemyKickBackForce, ForceMode.Impulse);
                enemyHealth.isDead = true;
            }
        }
        Destroy(this.gameObject);
    }

    // Funkcija kas pārbauda vai ir trāpija par galvu.
    bool IsHeadshot(Vector3 collisionPoint, GameObject head)
    {
        if (head == null)
        {
            Debug.LogWarning("Head GameObject not found in EnemyHealth script.");
            return false;
        }

        // Paskatās vai ir trāpijas tuvu.
        float distanceToHead = Vector3.Distance(head.transform.position, collisionPoint);
        float headshotDistanceThreshold = 0.5f;
        return distanceToHead <= headshotDistanceThreshold;
    }
}