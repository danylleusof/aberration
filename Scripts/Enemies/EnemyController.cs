using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    PlayerController playerController;

    [Header("Enemy Behaviour")]
    public float lookRadius = 10f;
    public float healthBarRadius = 10f;

    Transform target;
    NavMeshAgent agent;

    public Transform[] waypoints;
    int waypointIndex;
    Vector3 waypointTarget;

    [HideInInspector]
    public bool enemyPatrol, enemySpotted, enemyAttack;

    [Header("Enemy Attributes")]
    public float damageValue;
    public float knockbackForce = 10f;
    public float attackSpeed = 1f;
    float attackCooldown = 0f;
    Target isTarget;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        UpdateDestination();
        enemyAttack = false;

        playerController = player.GetComponent<PlayerController>();
        isTarget = GetComponent<Target>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius) 
        {
            agent.SetDestination(target.position);
            FaceTarget();

            if (distance <= agent.stoppingDistance)
                // Attack the target
                // Face the target
                AttackTarget();
            else
            {
                enemySpotted = true;
                enemyAttack = false;
            }
        }
        else
            UpdateDestination();


        if (Vector3.Distance(transform.position, waypointTarget) < 1)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }

        // Enable health bar when in range
        if (distance <= healthBarRadius)
        {
            isTarget.healthBar.gameObject.SetActive(true);
            CancelInvoke("DisableHealthBar");
        }
        else
            Invoke("DisableHealthBar", 1f);

        attackCooldown -= Time.deltaTime;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        enemyPatrol = false;
        enemySpotted = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void UpdateDestination()
    {
        waypointTarget = waypoints[waypointIndex].position;
        agent.SetDestination(waypointTarget);

        enemyPatrol = true;
        enemySpotted = false;
    }

    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
            waypointIndex = 0;
    }

    void AttackTarget()
    {
        if (attackCooldown <= 0f)
        {
            enemySpotted = false;
            enemyAttack = true;
            if (enemyAttack)
                playerController.TakeDamage(damageValue);
            attackCooldown = 1f / attackSpeed;
        }        
    }

    void DisableHealthBar()
    {
        isTarget.healthBar.gameObject.SetActive(false);
    }
}