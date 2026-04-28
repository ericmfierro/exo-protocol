using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class SentryAI : MonoBehaviour
{
    public float detectionRange = 12f;
    public float attackRange = 8f;

    private NavMeshAgent agent;
    private FirstPersonController player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<FirstPersonController>();

        agent.isStopped = true; // stays in place initially
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Player detected
        if (distance < detectionRange)
        {
            EngagePlayer(distance);
        }
        else
        {
            HoldPosition();
        }
    }

    void EngagePlayer(float distance)
    {
        // Face player
        transform.LookAt(player.transform);

        // Move only if needed
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;

            // Placeholder for attack
            Debug.Log("Sentry attacking player");
        }
    }

    void HoldPosition()
    {
        agent.isStopped = true;
    }
}