using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;
    //Meklēšana
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Uzbrukšana
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Pārbauda redzi un uzbrukšanas attālumu
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
        }
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint sasniegts
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack");

            float animationDuration = 1f;
            Invoke(nameof(DamagePlayer), animationDuration);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void DamagePlayer()
    {
        // Check if the player is still in attack range
        if (playerInAttackRange)
        {
            // Inflict damage to the player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(50);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void StopAnimations()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", false);
    }
}
