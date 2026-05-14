using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTime = 5f;
    public float fleeDistance = 8f;
    public float walkSpeed = 2f;
    public float sprintSpeed = 3f;
    public float maxHealth = 50f;
    public float damping = 0.15f;

    public bool IsHostage { get; private set; } = false;

    Animator anim;
    NavMeshAgent agent;
    float wanderTimer;
    Vector3 startPos;
    float currentHealth;
    bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        startPos = transform.position;
        wanderTimer = wanderTime;
    }

    void Update()
    {
        if (isDead) return;
        if (IsHostage) return;

        // Find closest enemy to flee from
        Robot closestEnemy = null;
        float closestDistance = 999f;
        Robot[] enemies = FindObjectsByType<Robot>(FindObjectsSortMode.None);

        foreach (Robot enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        bool shouldFlee = false;

        if (closestEnemy != null && closestDistance < fleeDistance)
        {
            shouldFlee = true;
            Vector3 fleeDir = transform.position - closestEnemy.transform.position;
            Vector3 fleeTarget = transform.position + fleeDir.normalized * fleeDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
            {
                agent.speed = sprintSpeed;
                agent.SetDestination(hit.position);
            }
        }
        else
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderTime)
            {
                Vector3 randomDir = Random.insideUnitSphere * wanderRadius + startPos;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDir, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    agent.speed = walkSpeed;
                    agent.SetDestination(hit.position);
                }
                wanderTimer = 0f;
            }
        }

        // Snap to idle if reached destination
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance + 0.1f)
        {
            anim.SetFloat("Speed", 0f, damping, Time.deltaTime);
            anim.SetFloat("MoveX", 0f, damping, Time.deltaTime);
            anim.SetFloat("MoveY", 0f, damping, Time.deltaTime);
            anim.SetBool("IsSprinting", false);
            return;
        }

        // Update animator with normalized direction
        float speed = agent.velocity.magnitude;
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity).normalized;

        if (Mathf.Abs(localVelocity.x) < 0.5f) localVelocity.x = 0f;
        if (localVelocity.z < 0f) localVelocity.z = Mathf.Abs(localVelocity.z);

        anim.SetFloat("Speed", speed, damping, Time.deltaTime);
        anim.SetFloat("MoveX", localVelocity.x, damping, Time.deltaTime);
        anim.SetFloat("MoveY", localVelocity.z, damping, Time.deltaTime);
        anim.SetBool("IsSprinting", shouldFlee);
    }

    public void TakeHostage(Transform captor)
    {
        if (isDead) return;

        IsHostage = true;
        anim.SetBool("IsHostage", true);
        agent.enabled = false;
        transform.SetParent(captor);
    }

    public void ReleaseHostage()
    {
        if (isDead) return;

        IsHostage = false;
        anim.SetTrigger("ReleaseHostage");
        anim.SetBool("IsHostage", false);
        agent.enabled = true;
        transform.SetParent(null);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("NPC took damage. Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetBool("IsDead", true);
            agent.enabled = false;
            Destroy(gameObject, 5f);
        }
    }
}
