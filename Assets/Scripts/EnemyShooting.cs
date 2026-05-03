using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform player;          // Assign in Inspector
    public float attackRange = 15f;
    public float fireRate = 1f;       // shots per second

    private Animator anim;
    private float fireCooldown;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 1. AIM LOGIC
        bool shouldAim = distance <= attackRange;
        anim.SetBool("isAiming", shouldAim);

        // 2. FIRE LOGIC
        if (shouldAim)
        {
            fireCooldown -= Time.deltaTime;

            if (fireCooldown <= 0f)
            {
                anim.SetTrigger("Fire");
                fireCooldown = 1f / fireRate;
            }
        }
    }
}