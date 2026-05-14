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

    [Header("Tracer")]
    public GameObject tracerPrefab;
    public Transform firePoint;
    public float tracerDistance = 40f;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;

    [Header("Audio")]
    public AudioSource gunAudioSource;
    public AudioClip firingClip;

    [Header("Rotation")]
    public float turnSpeed = 15f;

    [Tooltip("Use this if the enemy model/fire animation is not visually facing the player. Try 0, 90, -90, or 180.")]
    public float firingRotationOffset = 0f;

    Transform player;
    PlayerStats playerStats;
    Animator anim;
    NavMeshAgent agent;

    float nextFireTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        FirstPersonController fpc =
            FindFirstObjectByType<FirstPersonController>();

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
            StopCombat();
            return;
        }

        float distance =
            Vector3.Distance(transform.position, player.position);

        bool shouldAttack =
            distance <= attackRange;

        if (anim != null)
        {
            anim.SetBool("Fire", shouldAttack);
        }

        if (shouldAttack)
        {
            if (agent != null && agent.enabled)
            {
                agent.isStopped = true;
                agent.updateRotation = false;
            }

            RotateTowardPlayer();

            StartGunAudio();

            if (Time.time >= nextFireTime)
            {
                FireShot();

                nextFireTime =
                    Time.time + (1f / fireRate);
            }
        }
        else
        {
            StopCombat();
        }
    }

    void RotateTowardPlayer()
    {
        if (player == null)
            return;

        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion lookRotation =
            Quaternion.LookRotation(direction.normalized);

        Quaternion offsetRotation =
            Quaternion.Euler(0f, firingRotationOffset, 0f);

        Quaternion targetRotation =
            lookRotation * offsetRotation;

        transform.rotation =
            Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * 120f * Time.deltaTime
            );
    }

    void FireShot()
    {
        if (firePoint == null)
            return;

        if (muzzleFlash != null)
        {
            muzzleFlash.Emit(1);
        }

        Vector3 startPos =
            firePoint.position;

        Vector3 targetPos =
            player.position + Vector3.up * 1.2f;

        Vector3 direction =
            (targetPos - startPos).normalized;

        Vector3 endPos =
            startPos + direction * tracerDistance;

        if (tracerPrefab != null)
        {
            GameObject tracerObj =
                Instantiate(tracerPrefab);

            Tracer tracer =
                tracerObj.GetComponent<Tracer>();

            if (tracer != null)
            {
                tracer.Setup(startPos, endPos);
            }
        }

        if (Random.value <= hitChance &&
            playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }
    }

    void StartGunAudio()
    {
        if (gunAudioSource != null &&
            firingClip != null &&
            !gunAudioSource.isPlaying)
        {
            gunAudioSource.clip = firingClip;
            gunAudioSource.loop = true;
            gunAudioSource.Play();
        }
    }

    void StopGunAudio()
    {
        if (gunAudioSource != null &&
            gunAudioSource.isPlaying)
        {
            gunAudioSource.Stop();
        }
    }

    void StopCombat()
    {
        if (anim != null)
        {
            anim.SetBool("Fire", false);
        }

        StopGunAudio();

        if (agent != null && agent.enabled)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
        }
    }
}
