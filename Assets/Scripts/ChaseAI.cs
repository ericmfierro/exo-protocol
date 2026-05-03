using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class ChaseAI : MonoBehaviour
{
    public float detectionRange = 20f;

    NavMeshAgent agent;
    FirstPersonController player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<FirstPersonController>();
    }

    void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < detectionRange)
        {
            agent.SetDestination(player.transform.position);
        }
    }
}