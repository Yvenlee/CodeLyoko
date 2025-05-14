using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovements : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;

    public float chaseSpeed = 5f;
    public float stoppingDistance = 1.5f;
    public float updateInterval = 0.5f;
    private float nextUpdateTime;
    private bool isChasing = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Mettre à jour la destination à intervalles réguliers
        if (isChasing && Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateInterval;

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > stoppingDistance)
            {
                agent.SetDestination(playerTransform.position);
            }
            else
            {
                agent.ResetPath();
            }
            FacePlayer();
        }
    }

    public void StartChasingPlayer()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
    }

    public void StopChasingPlayer()
    {
        isChasing = false;
        agent.ResetPath();
    }

    private void FacePlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
