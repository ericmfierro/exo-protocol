using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class EnemyShooter : MonoBehaviour
{
    [Header("Combat")]
    public float attackRange = 15f;
    public float fireRate = 10f;
    public float damage = 2f;

    [Header("Accuracy")]
    [Range(0f, 1f)]
    public float hitChance = 0.65f;
    public float horizontalSpread = 1.5f;
    public float verticalSpread = 0.4f;

    [Header("Tracer")]
    public GameObject tracerPrefab;
    public Transform firePoint;
    public float tracerDistance = 40f;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;

    [Header("Rotation")]
    public float turnSpeed = 8f;

    Transform player;
    PlayerStats playerStats;
    Animator anim;
    NavMeshAgent agent;
    float nextFireTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        FirstPersonController fpc = FindFirstObjectByType<FirstPersonController>();
        if (fpc != null)
        {
            player = fpc.transform;
            playerStats = fpc.GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        if (player == null)
        {
            StopFiring();
            return;
        }

        if (playerStats != null && playerStats.currentHealth <= 0)
        {
            StopFiring();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        bool shouldAttack = distance <= attackRange;

        anim.SetBool("Fire", shouldAttack);

        if (shouldAttack)
        {
            // Stop the agent so we don't slide while shooting
            if (agent != null)
            {
                agent.isStopped = true;
                agent.updateRotation = false;
            }

            RotateTowardPlayer();

            if (Time.time >= nextFireTime)
            {
                FireShot();
                nextFireTime = Time.time + (1f / fireRate);
            }
        }
        else
        {
            StopFiring();
        }
    }

    void RotateTowardPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void FireShot()
    {
        if (firePoint == null || tracerPrefab == null) return;

        if (muzzleFlash != null) muzzleFlash.Emit(1);

        Vector3 startPos = firePoint.position;
        Vector3 targetPos = player.position + Vector3.up * 1.2f;

        Vector3 spread = new Vector3(
            Random.Range(-horizontalSpread, horizontalSpread),
            Random.Range(-verticalSpread, verticalSpread),
            Random.Range(-horizontalSpread, horizontalSpread)
        );
        targetPos += spread;

        Vector3 direction = (targetPos - startPos).normalized;
        Vector3 endPos = startPos + direction * tracerDistance;

        GameObject tracerObj = Instantiate(tracerPrefab);
        Tracer tracer = tracerObj.GetComponent<Tracer>();
        if (tracer != null)
        {
            tracer.Setup(startPos, endPos);
        }

        if (Random.value <= hitChance && playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }
    }

    void StopFiring()
    {
        if (anim != null) anim.SetBool("Fire", false);

        if (agent != null)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
        }
    }
}
