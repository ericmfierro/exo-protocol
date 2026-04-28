using UnityEngine;
using UnityEngine.AI;

public class EliteAI : MonoBehaviour
{
    public float lowHealthThreshold = 20f;
    public float hostageKillTime = 5f;

    private Robot robot;
    private NavMeshAgent agent;

    private Civilian targetCivilian;
    private Civilian heldCivilian;

    private float killTimer = 0f;
    private bool hasHostage = false;

    void Start()
    {
        robot = GetComponent<Robot>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (robot == null) return;

        // If low health → find hostage
        if (!hasHostage && robot.GetHealthPercent() < 0.3f)
        {
            FindClosestCivilian();
        }

        if (targetCivilian != null && !hasHostage)
        {
            agent.SetDestination(targetCivilian.transform.position);

            float dist = Vector3.Distance(transform.position, targetCivilian.transform.position);

            if (dist < 2f)
            {
                GrabHostage();
            }
        }

        if (hasHostage)
        {
            killTimer += Time.deltaTime;

            if (killTimer >= hostageKillTime)
            {
                ExecuteHostage();
            }
        }
    }

    bool robotHealthLow()
    {
        return robot != null && robotHealthPercent() < lowHealthThreshold;
    }

    float robotHealthPercent()
    {
        return robot != null ? robotHealth() : 0f;
    }

    float robotHealth()
    {
        // Track health inside Robot later if needed
        return robot.GetComponent<Robot>().GetComponent<Robot>().maxHealth; // can improve later
    }

    void FindClosestCivilian()
    {
        Civilian[] civilians = FindObjectsByType<Civilian>(FindObjectsSortMode.None);

        float closestDist = Mathf.Infinity;

        foreach (Civilian civ in civilians)
        {
            if (civ.isCaptured) continue;

            float dist = Vector3.Distance(transform.position, civ.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                targetCivilian = civ;
            }
        }
    }

    void GrabHostage()
    {
        if (targetCivilian == null) return;

        heldCivilian = targetCivilian;
        heldCivilian.Capture(transform);

        hasHostage = true;
        killTimer = 0f;

        targetCivilian = null;

        Debug.Log("Hostage captured!");
    }

    void ExecuteHostage()
    {
        if (heldCivilian != null)
        {
            heldCivilian.Die();
        }

        hasHostage = false;
        heldCivilian = null;
    }
}