using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CloseEnemy : MonoBehaviour
{
    NavMeshAgent pathfinder;
    Transform target;
    private float maxHealth = 70f;
    public float health;

    Material skinMaterial;
    Color originalColour;

    float attackDistanceThreshold = 0.5f;
    float timeBetweenAttacks = 1;

    
    float myCollisionRadius;
    float targetCollisionRadius;

    void Start()
    {
        pathfinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;

        // Check if the NavMeshAgent is enabled and on the NavMesh
        if (pathfinder.isActiveAndEnabled && pathfinder.isOnNavMesh)
        {
            StartCoroutine(UpdatePath());
        }

        skinMaterial = GetComponent<Renderer>().material;
        originalColour = skinMaterial.color;

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
    }

    private bool isAttacking = false;
private bool hasDealtDamage = false;
private float attackCooldown = 1f;
private float nextAttackTime = 0f;

void Update()
{
    if (!isAttacking && Time.time > nextAttackTime)
    {
        float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
        if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
        {
            StartCoroutine(Attack());
            nextAttackTime = Time.time + attackCooldown;
            hasDealtDamage = false; // Reset the flag when starting a new attack
        }
    }
}

IEnumerator Attack()
{
    isAttacking = true;
    hasDealtDamage = false;

    pathfinder.enabled = false;

    Vector3 originalPosition = transform.position;
    Vector3 dirToTarget = (target.position - transform.position).normalized;
    Vector3 attackPosition = target.position - dirToTarget * myCollisionRadius;

    float attackSpeed = 3;
    float percent = 0;

    skinMaterial.color = Color.red;

    while (percent <= 1)
    {
        percent += Time.deltaTime * attackSpeed;
        float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
        transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

        // Check for player proximity during the attack animation
        float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
        if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2) && !hasDealtDamage)
        {
            // Check if the player is within range and apply damage
            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                player.damage(10);
                hasDealtDamage = true; // Set the flag to true once damage is applied
            }
        }

        yield return null;
    }

    skinMaterial.color = originalColour;

    pathfinder.enabled = true;
    isAttacking = false;
}


    public void damage(float dmg)
    {
        if (health <= 0) return;
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 0.25f;
        float detectionRange = 20f; 

        while (target != null)
        {
            float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

            if (sqrDstToTarget < Mathf.Pow(detectionRange, 2))
            {
                Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);

                if (pathfinder.isActiveAndEnabled && pathfinder.isOnNavMesh)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }

}
