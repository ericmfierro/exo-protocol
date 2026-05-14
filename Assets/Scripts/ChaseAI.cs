using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class ChaseAI : MonoBehaviour
{
    public float detectionRange = 20f;

    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float sprintSpeed = 5.5f;

    [Header("Patrol")]
    public float patrolRadius = 15f;
    public float patrolWaitTime = 3f;

    NavMeshAgent agent;
    Transform player;
    HostageGrabber grabber;
    EnemyShooter shooter;

    Vector3 startPos;
    float waitTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        grabber = GetComponent<HostageGrabber>();
        shooter = GetComponent<EnemyShooter>();

        startPos = transform.position;

        waitTimer = patrolWaitTime;

        FirstPersonController fpc =
            FindFirstObjectByType<FirstPersonController>();

        if (fpc != null)
        {
            player = fpc.transform;
        }
    }

    void Update()
    {
        if (player == null ||
            agent == null ||
            !agent.enabled)
        {
            return;
        }

        if (grabber != null &&
            grabber.HasHostage)
        {
            agent.isStopped = true;
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // Close range = aggressive sprint
        if (distance < 8f)
        {
            agent.speed = sprintSpeed;
        }
        // Medium range = run
        else if (distance < detectionRange)
        {
            agent.speed = runSpeed;
        }
        // Patrol
        else
        {
            agent.speed = walkSpeed;
        }

        if (distance < detectionRange)
        {
            agent.SetDestination(
                player.position
            );
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!agent.pathPending &&
            agent.remainingDistance < 1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= patrolWaitTime)
            {
                Vector3 randomDir =
                    Random.insideUnitSphere *
                    patrolRadius +
                    startPos;

                NavMeshHit hit;

                if (NavMesh.SamplePosition(
                    randomDir,
                    out hit,
                    patrolRadius,
                    NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }

                waitTimer = 0f;
            }
        }
    }
}