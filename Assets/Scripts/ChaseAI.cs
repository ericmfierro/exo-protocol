using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class ChaseAI : MonoBehaviour
{
    public float detectionRange = 20f;

    private Transform player;
    private NavMeshAgent agent;
    private EnemyShooter shooter;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        shooter = GetComponent<EnemyShooter>();

        // Prevent NavMeshAgent from fighting rotation
        if (agent != null)
        {
            agent.updateRotation = false;
        }

        // Auto-find player
        FirstPersonController fpc =
            FindFirstObjectByType<FirstPersonController>();

        if (fpc != null)
        {
            player = fpc.transform;
        }
    }

    void Update()
    {
        if (player == null || agent == null)
            return;

        float distance =
            Vector3.Distance(transform.position, player.position);

        // If enemy is shooting, stop chasing
        if (shooter != null &&
            distance <= shooter.attackRange)
        {
            agent.isStopped = true;
            return;
        }

        // Resume movement
        agent.isStopped = false;

        // Chase player
        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);
        }
    }
}