using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy;//Laks līdz lode tiek iznīcināta.
    [HideInInspector] public WeaponManager weapon;
    [HideInInspector] public Vector3 dir;

    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    //Metode, kas notiek, kad lode saskaras ar pretinieku
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();

            
            GameObject head = enemyHealth.head;

            // Pārbauda vai ir trāpijis par galvu
            bool isHeadshot = IsHeadshot(collision.contacts[0].point, head);

            //Atņem pretiniekam vajadzīgās dzīvības daudzumu.
            enemyHealth.TakeDamage(weapon.damage, isHeadshot);
            //Ja pretinieka dzīvības ir nulle un pretinieks nav miris, tad aktivē ragdoll un pataisa pretinieku mirušu
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
            Debug.LogWarning("Galvas GameObject nav atrasts!");
            return false;
        }

        // Paskatās vai ir trāpijas tuvu.
        float distanceToHead = Vector3.Distance(head.transform.position, collisionPoint);
        float headshotDistanceThreshold = 0.5f;
        return distanceToHead <= headshotDistanceThreshold;
    }
}