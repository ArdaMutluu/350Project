using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public float chaseRange = 10f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    public GameObject ea;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private bool canAttack = true;
    private bool isAttackCooldown = false;
    public Animator anim;
    private bool isPerformingAttack2 = false;
    public float maxHealth = 100f;
    public float health;
    public GameObject fist;
    public GameObject explosion;
    public GameObject healthUI;
    public GameObject nextleve;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = attackRange;
        health = maxHealth;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isPerformingAttack2)
        {
            if (!isAttackCooldown)
            {
                if (distanceToPlayer < attackRange)
                {
                    StopChasing();
                    Attack();
                }
                else if (distanceToPlayer < chaseRange)
                {
                    ChasePlayer();
                }
                else
                {
                    StopChasing();
                }
            }
        }
    }

    void ChasePlayer()
    {
        if (navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }

        navMeshAgent.SetDestination(player.position);
        anim.SetBool("isidle", false);
        anim.SetBool("iswalk", true);
    }

    void StopChasing()
    {
        if (!isAttackCooldown)
        {
            navMeshAgent.isStopped = true;
            anim.SetBool("iswalk", false);
        }
    }

    void Attack()
    {
        if (canAttack)
        {
            float attackProbability = Random.value;

            if (attackProbability <= 0.7f)
            {
                Attack1();
            }
            else
            {
                Attack2();
            }

            canAttack = false;
            isAttackCooldown = true;
            Invoke("ResetAttackCooldown", attackCooldown);
        }
    }

    void Attack1()
    {
        Debug.Log("Boss performs Attack Type 1!");
        SphereCollider fistCollider = fist.GetComponent<SphereCollider>();
        GetComponent<Animator>().Play("Punch");
    }

    void Attack2()
    {
        Debug.Log("Boss performs Attack Type 2!");
        StartCoroutine(PerformAttack2());
    }

    IEnumerator PerformAttack2()
    {
        isPerformingAttack2 = true;
        isAttackCooldown = true;

        navMeshAgent.isStopped = true;
        anim.SetBool("iswalk", false);

        ea.SetActive(true);

        yield return new WaitForSeconds(2f);

        ea.SetActive(false);
        navMeshAgent.isStopped = false;
        anim.SetBool("iswalk", true);

        isPerformingAttack2 = false;
    }

    public void damage(float dmg)
    {
        if (health <= 0) return;
        health -= dmg;
        if (health <= 0)
        {
            StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        explosion.SetActive(true);
        healthUI.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        nextleve.SetActive(true);
    }

    void ResetAttackCooldown()
    {
        if (ea == true)
        {
            ea.SetActive(false);
        }
        
        canAttack = true;
        isAttackCooldown = false;

        ChasePlayer();
    }
}
