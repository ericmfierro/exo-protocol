using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class EnemyShooter : MonoBehaviour
{
    [Header("Combat")]
    public Transform player;
    public float attackRange = 15f;
    public float fireRate = 10f;
    public float damage = 5f;

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

    [Header("Audio")]
    public AudioSource gunAudioSource;
    public AudioClip firingClip;

    [Header("Movement")]
    public bool stopMovementWhileFiring = true;

    [Header("Rotation")]
    public float turnSpeed = 8f;

    // Adjust depending on model facing
    public Vector3 modelRotationOffset =
        new Vector3(0f, 60f, 0f);

    private PlayerStats playerStats;
    private Animator anim;
    private NavMeshAgent agent;

    private float nextFireTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Prevent NavMeshAgent rotation conflicts
        if (agent != null)
        {
            agent.updateRotation = false;
        }

        // Auto-find player
        if (player == null)
        {
            FirstPersonController fpc =
                FindFirstObjectByType<FirstPersonController>();

            if (fpc != null)
            {
                player = fpc.transform;
            }
        }

        // Get player stats
        if (player != null)
        {
            playerStats =
                player.GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        // PLAYER MISSING
        if (player == null)
        {
            StopCombatAnimations();
            StopGunAudio();
            return;
        }

        // PLAYER DEAD
        if (playerStats != null &&
            playerStats.currentHealth <= 0)
        {
            StopCombatAnimations();
            StopGunAudio();
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        bool shouldAttack =
            distance <= attackRange;

        // Combat animation state
        anim.SetBool(
            "Fire",
            shouldAttack
        );

        if (shouldAttack)
        {
            // Stop movement while firing
            if (agent != null &&
                stopMovementWhileFiring)
            {
                agent.isStopped = true;
            }

            RotateTowardPlayer();

            // START GUN AUDIO LOOP
            if (gunAudioSource != null &&
                firingClip != null)
            {
                if (!gunAudioSource.isPlaying)
                {
                    gunAudioSource.clip =
                        firingClip;

                    gunAudioSource.loop = true;

                    gunAudioSource.pitch =
                        Random.Range(0.95f, 1.05f);

                    gunAudioSource.Play();
                }
            }

            // Automatic fire timing
            if (Time.time >= nextFireTime)
            {
                FireShot();

                nextFireTime =
                    Time.time +
                    (1f / fireRate);
            }
        }
        else
        {
            StopCombatAnimations();
            StopGunAudio();
        }
    }

    void RotateTowardPlayer()
    {
        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction.normalized
            ) *
            Quaternion.Euler(
                modelRotationOffset
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
    }

    void FireShot()
    {
        if (player == null ||
            firePoint == null ||
            tracerPrefab == null)
        {
            return;
        }

        // MUZZLE FLASH
        if (muzzleFlash != null)
        {
            muzzleFlash.Emit(1);
        }

        Vector3 startPos =
            firePoint.position;

        // Aim near upper torso
        Vector3 targetPos =
            player.position +
            Vector3.up * 1.2f;

        // Spread
        Vector3 spread =
            new Vector3(
                Random.Range(
                    -horizontalSpread,
                    horizontalSpread
                ),

                Random.Range(
                    -verticalSpread,
                    verticalSpread
                ),

                Random.Range(
                    -horizontalSpread,
                    horizontalSpread
                )
            );

        targetPos += spread;

        Vector3 direction =
            (targetPos - startPos)
            .normalized;

        Vector3 endPos =
            startPos +
            direction *
            tracerDistance;

        // SPAWN TRACER
        GameObject tracerObj =
            Instantiate(tracerPrefab);

        Tracer tracer =
            tracerObj.GetComponent<Tracer>();

        if (tracer != null)
        {
            tracer.Setup(
                startPos,
                endPos
            );
        }

        // DAMAGE
        if (Random.value <= hitChance)
        {
            if (playerStats != null)
            {
                playerStats.TakeDamage(
                    damage
                );
            }
        }
    }

    void StopCombatAnimations()
    {
        anim.SetBool(
            "Fire",
            false
        );

        if (agent != null)
        {
            agent.isStopped = false;
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
}