using UnityEngine;
using StarterAssets;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    FirstPersonController player;
    NavMeshAgent agent;

    // Awake: Grab components ON this GameObject
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start: grab references from OTHER GameObjects
    void Start()
    {
        player = FindFirstObjectByType<FirstPersonController>();
    }

    // Update destination every frame so robot follows player
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }

}
