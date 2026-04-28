using UnityEngine;
using UnityEngine.AI;

public class AmbushAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Civilian targetCivilian;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindTarget();
    }

    void Update()
    {
        if (targetCivilian == null)
        {
            FindTarget();
            return;
        }

        agent.SetDestination(targetCivilian.transform.position);
    }

    void FindTarget()
    {
        Civilian[] civilians = FindObjectsByType<Civilian>(FindObjectsSortMode.None);

        if (civilians.Length > 0)
        {
            targetCivilian = civilians[Random.Range(0, civilians.Length)];
        }
    }
}