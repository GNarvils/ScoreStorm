using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (collision.gameObject.GetComponentInParent<EnemyHealth>()){
            EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(weapon.damage);

            if (enemyHealth.health<=0 && enemyHealth.isDead == false) {
                Rigidbody rb = collision.gameObject.GetComponentInChildren<Rigidbody>();
                rb.AddForce(dir * weapon.enemyKickBackForce, ForceMode.Impulse);
                enemyHealth.isDead = true;
            }
        }
        Destroy(this.gameObject);
    }
}
