using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class PatrolAI : MonoBehaviour
{
    public Transform[] waypoints;

    public float detectionRange = 10f;

    [Header("Movement")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3.5f;

    int currentIndex = 0;

    NavMeshAgent agent;
    FirstPersonController player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player =
            FindFirstObjectByType<FirstPersonController>();

        if (agent != null)
        {
            agent.speed = patrolSpeed;
        }

        GoToNextPoint();
    }

    void Update()
    {
        if (player == null ||
            agent == null ||
            !agent.enabled)
        {
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.transform.position
            );

        if (distance < detectionRange)
        {
            agent.speed = chaseSpeed;

            agent.SetDestination(
                player.transform.position
            );
        }
        else
        {
            agent.speed = patrolSpeed;

            Patrol();
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0)
            return;

        if (!agent.pathPending &&
            agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination =
            waypoints[currentIndex].position;

        currentIndex =
            (currentIndex + 1) %
            waypoints.Length;
    }
}