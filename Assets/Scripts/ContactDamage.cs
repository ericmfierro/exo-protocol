using UnityEngine;
using StarterAssets;

/// Deals damage to the player when the robot is close

public class ContactDamage : MonoBehaviour
{
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float damageCooldown = 1f;
    [SerializeField] float damageRange = 2f;

    float lastDamageTime;
    Transform player;
    PlayerStats playerStats;

    void Start()
    {
        FirstPersonController fpc = FindFirstObjectByType<FirstPersonController>();
        if (fpc != null)
        {
            player = fpc.transform;
            playerStats = fpc.GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        if (player == null || playerStats == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < damageRange && Time.time > lastDamageTime + damageCooldown)
        {
            lastDamageTime = Time.time;
            playerStats.TakeDamage(damagePerHit);
            Debug.Log("Robot hit player for " + damagePerHit + " damage");
        }
    }
}