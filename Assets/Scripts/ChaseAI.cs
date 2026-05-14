using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class ChaseAI : MonoBehaviour
{
    public float detectionRange = 20f;
    public float patrolRadius = 15f;
    public float patrolWaitTime = 3f;
    public float chaseSpeed = 3.5f;
    public float patrolSpeed = 1.5f;

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

        FirstPersonController fpc = FindFirstObjectByType<FirstPersonController>();
        if (fpc != null)
        {
            player = fpc.transform;
        }
    }

    void Update()
    {
        if (player == null || agent == null) return;

        // Don't chase if holding a hostage
        if (grabber != null && grabber.HasHostage)
        {
            agent.isStopped = true;
            return;
        }

        // Don't chase if firing (EnemyShooter handles its own stopping)
        // The shooter sets isStopped while firing; we don't override
        if (agent.isStopped) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.speed = patrolSpeed;
            Patrol();
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= patrolWaitTime)
            {
                Vector3 randomDir = Random.insideUnitSphere * patrolRadius + startPos;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDir, out hit, patrolRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
                waitTimer = 0f;
            }
        }
    }
}
