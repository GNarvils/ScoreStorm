using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class EnemyAi : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player; //Spēlētajs

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;

    public int damageToPlayer; // Cik daudz dzīvības atņems spēlētājam

    private bool staggered = false; //Vai pretinieks ir apstādināts

    public GameObject spell; //Uguns bumba
    public bool hasSpells = true; //Vai var mest uguns bumbas

    //Kad pretinieks nav atradis spēlētāji viņš staigā uz riņķa notiektajā rādijusā
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks; // Laiks starp uzbrukumiem
    bool alreadyAttacked; // Vai nav jau uzbrucis

    public float sightRange, attackRange; // Attālums, kur pretinieks redz spēlētāju un attālums, kur pretinieks var uzbrukt.
    public bool playerInSightRange, playerInAttackRange; //Vai spēlētāju redzu un vai var viņam uzbrukt

    private bool spotSoundPlayed = false; //Vai spēlētāja redzēšanas skaņa bija spēlēta

    public bool hasBeenHit = false; //Vai spēlētājs ir iešāvis pretiniekam

    public EnemySpawn enemySpawn;

    private void Awake()
    {
        //Dabū spēlētāja objektu, kā arī parādīšanās komponentu
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
        agent.speed = Random.Range(3f, 7.5f);

        GameObject enemiesObject = GameObject.Find("Enemies");
        enemySpawn = enemiesObject.GetComponent<EnemySpawn>();
        
      

    }

    private void Update()
    {
        //Ja pretinieks ir apstādināts izej ārā
        if (staggered)
            return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        //Ja spēlētajs ir nomiris, tad beidz viņam sekot
        if (playerHealth != null && playerHealth.isDead)
        {
            Patroling();
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
            return;
        }

        //Ja ir nošauti  10 pretinieki, tad viņi visu laiku sekos spēlētājam 
        if (enemySpawn != null && enemySpawn.killedEnemy >= 10)
        {
            hasBeenHit = true;
        }
        //Ja ir nošauti 50 pretinieki, viņu ātrums palielināsies uz gandrīz maksimālo iešanas ātrumu
        else if (enemySpawn != null && enemySpawn.killedEnemy >= 50) {
            agent.speed = 6.5f;
        }
        //Ja ir nošauti 75 pretinieki, viņu ātrums palielināsies uz maksimālo iešanas ātrumu
        else if (enemySpawn != null && enemySpawn.killedEnemy >= 75)
        {
            agent.speed = 7.5f;
        }



        //Pārbauda redzi un uzbrukšanas attālumu
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        //Ja spēlētāju neredz un nav uzbrukšanas attāluma, tad staigā uz vietas
        if (!playerInSightRange && !playerInAttackRange || !playerInAttackRange && !hasBeenHit)
        {
            Patroling();
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }
        //Ja spēlētāju redz, bet nevar uzbrukt sāc sekot
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
                    //Spēlē skaņu
                    enemyHealth.EnemySpottedSound();
                    spotSoundPlayed = true;
                }
            }

        }
        //Uzbrūc spēlētājam ja redz un ir tuvu
        if (playerInSightRange && playerInAttackRange || hasBeenHit && playerInAttackRange)
        {
            //Ja var mest uguns bumbas, tad met, bet ja nē tad uzbrūc fiziski
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
    //Staigā uz vietas noteiktā rādiusā
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }
    //Meklē nākošo ceļu, kamēr iet uz vietas
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }
    //Seko spēlētājam
    private void ChasePlayer()
    {
        animator.SetBool("Reaction", false);
        agent.SetDestination(player.position);
    }
    //Uzbrūc fiziski spēlētājam
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
    //Parāda ugunsbumbu, pēc īsa laika
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
    //Uzbrūc spēlētājam izmantojot ugunsbumbas
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

    //Atņem dzīvības spēlētājam
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
    //Restartē uzbrukumu
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    //Apstādina pretinieku
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
    //Pēc apstādināšanas sāk atkal iet
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
    //Apstādina animācijas pretinieka
    public void StopAnimations()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Reaction", false);
    }


}
