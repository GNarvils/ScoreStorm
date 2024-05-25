using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class EnemyAi : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;

    public int damageToPlayer;

    private bool staggered = false;

    public GameObject spell;
    public bool hasSpells = true;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private bool spotSoundPlayed = false;

    public bool hasBeenHit = false;

    public GameObject enemyType;


    private void Awake()
    {
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);
        GameObject selectedPlayerObject = null;

        if (selectedPlayer == 1)
        {
            selectedPlayerObject = GameObject.Find("Player_1");
        }
        else if (selectedPlayer == 2)
        {
            selectedPlayerObject = GameObject.Find("Player_2");
        }
        player = selectedPlayerObject.transform;
        agent = GetComponent<NavMeshAgent>();
        enemyType = this.gameObject;
        agent.speed = Random.Range(1f, 7.5f);

    }

    private void Update()
    {
        if (staggered)
            return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.isDead)
        {
            Patroling();
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
            return;
        }



        //Pārbauda redzi un uzbrukšanas attālumu
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange || !playerInAttackRange && !hasBeenHit)
        {
            Patroling();
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }
        if (playerInSightRange && !playerInAttackRange || !playerInAttackRange && hasBeenHit)
        {
            ChasePlayer();
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
            if (!spotSoundPlayed) 
            {
                EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.EnemySpottedSound();
                    spotSoundPlayed = true;
                }
            }

        }
        if (playerInSightRange && playerInAttackRange || hasBeenHit && playerInAttackRange)
        {
            if (hasSpells)
            {
                ProjectileAttackPlayer();
            }
            else
            {
                MeleeAttackPlayer();
            }
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
        }
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

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
        animator.SetBool("Reaction", false);
        agent.SetDestination(player.position);
    }

    private void MeleeAttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.EnemyAttackSound();
            }
            animator.SetTrigger("Attack");

            float animationDuration = 1f;
            Invoke(nameof(DamagePlayer), animationDuration);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private IEnumerator SpawnProjectileWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 spawnPosition = transform.position + transform.forward * 1.5f + transform.up * 1.5f;
        Rigidbody rb = Instantiate(spell, spawnPosition, Quaternion.identity).GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        Vector3 projectileDirection = transform.forward + Vector3.up * 0.25f;
        float forceMagnitude = 15f;
        rb.AddForce(projectileDirection * forceMagnitude, ForceMode.Impulse);

    }

    private void ProjectileAttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.EnemyAttackSound();
            }
            animator.SetTrigger("Attack");

            StartCoroutine(SpawnProjectileWithDelay(0.5f));
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void DamagePlayer()
    {
        if (playerInAttackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            ActionStateManager actions = player.GetComponentInChildren<ActionStateManager>();
            if (playerHealth != null)
            {
                if (actions.currentState == actions.Guard) playerHealth.TakeDamage(damageToPlayer / 2);
                else playerHealth.TakeDamage(damageToPlayer);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void Stagger()
    {
        agent.isStopped = true;
        staggered = true;
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Reaction", true);
        Debug.Log("Enemy staggered");

        StartCoroutine(RecoverFromStagger());
    }

    private IEnumerator RecoverFromStagger()
    {
        yield return new WaitForSeconds(1.2f);
        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null && !enemyHealth.isDead)
        {
            staggered = false;
            agent.isStopped = false;
            animator.SetBool("Reaction", false);
        }
    }
    public void StopAnimations()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Reaction", false);
    }


}
