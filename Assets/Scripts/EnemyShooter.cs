using UnityEngine;
using StarterAssets;

public class EnemyShooter : MonoBehaviour
{
    public Transform player;
    public float attackRange = 15f;
    public float fireRate = 1f;
    public float damage = 5f;

    PlayerStats playerStats;
    Animator anim;
    float fireCooldown;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Find the player automatically if not assigned
        if (player == null)
        {
            FirstPersonController fpc = FindFirstObjectByType<FirstPersonController>();
            if (fpc != null)
            {
                player = fpc.transform;
            }
        }

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool shouldAim = distance <= attackRange;

        anim.SetBool("IsAiming", shouldAim);
        anim.SetBool("IsInCombat", shouldAim);

        if (shouldAim)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

            fireCooldown -= Time.deltaTime;

            if (fireCooldown <= 0f)
            {
                anim.SetBool("IsFiring", true);

                if (playerStats != null) playerStats.TakeDamage(damage);

                fireCooldown = 1f / fireRate;
                Invoke(nameof(StopFiring), 0.3f);
            }
        }
        else
        {
            anim.SetBool("IsFiring", false);
        }
    }

    void StopFiring()
    {
        anim.SetBool("IsFiring", false);
    }
}
