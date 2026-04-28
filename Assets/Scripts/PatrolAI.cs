using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class PatrolAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float detectionRange = 10f;

    private int currentIndex = 0;
    private NavMeshAgent agent;
    private FirstPersonController player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<FirstPersonController>();

        GoToNextPoint();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // If player is close → chase
        if (distance < detectionRange)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        agent.destination = waypoints[currentIndex].position;
        currentIndex = (currentIndex + 1) % waypoints.Length;
    }
}