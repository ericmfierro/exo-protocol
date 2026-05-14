using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class HostageGrabber : MonoBehaviour
{
    public float hostageSearchRange = 25f;
    public float grabDistance = 3f;
    public float playerDetectRange = 15f;
    public float hostageOffset = 0.7f;

    public bool HasHostage => grabbedHostage != null;

    NavMeshAgent agent;
    Animator anim;
    Transform player;
    NPCController grabbedHostage;
    NPCController targetHostage;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        FirstPersonController fpc = FindFirstObjectByType<FirstPersonController>();
        if (fpc != null)
        {
            player = fpc.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Already holding hostage - do nothing
        if (grabbedHostage != null) return;

        // Look for hostage when player is close
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < playerDetectRange && targetHostage == null)
        {
            targetHostage = FindClosestNPC();
        }

        if (targetHostage != null)
        {
            // Drop target if another grabber got them first
            if (targetHostage.IsHostage)
            {
                targetHostage = null;
                return;
            }

            float distanceToHostage = Vector3.Distance(transform.position, targetHostage.transform.position);

            if (distanceToHostage > grabDistance)
            {
                agent.SetDestination(targetHostage.transform.position);
            }
            else
            {
                GrabHostage();
            }
        }
    }

    NPCController FindClosestNPC()
    {
        NPCController[] npcs = FindObjectsByType<NPCController>(FindObjectsSortMode.None);
        NPCController closest = null;
        float closestDistance = hostageSearchRange;

        foreach (NPCController npc in npcs)
        {
            if (npc.IsHostage) continue;

            float distance = Vector3.Distance(transform.position, npc.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = npc;
            }
        }
        return closest;
    }

    void GrabHostage()
    {
        if (targetHostage == null) return;

        grabbedHostage = targetHostage;
        targetHostage = null;

        grabbedHostage.TakeHostage(transform);
        grabbedHostage.transform.position = transform.position + transform.forward * hostageOffset;
        grabbedHostage.transform.rotation = transform.rotation;

        if (anim != null) anim.SetTrigger("GrabHostage");

        agent.isStopped = true;

        Debug.Log(name + " grabbed " + grabbedHostage.name);
    }

    void OnDestroy()
    {
        if (grabbedHostage != null)
        {
            grabbedHostage.ReleaseHostage();
        }
    }
}
