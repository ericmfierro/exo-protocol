using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 50f;

    [SerializeField]
    private Slider healthSlider;

    private float currentHealth;

    private Animator anim;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();

        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value =
                currentHealth / maxHealth;
        }
    }

    void Die()
    {
        isDead = true;

        // STOP NAVMESH
        NavMeshAgent agent =
            GetComponent<NavMeshAgent>();

        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // STOP AI
        ChaseAI chaseAI =
            GetComponent<ChaseAI>();

        if (chaseAI != null)
        {
            chaseAI.enabled = false;
        }

        PatrolAI patrolAI =
            GetComponent<PatrolAI>();

        if (patrolAI != null)
        {
            patrolAI.enabled = false;
        }

        // STOP SHOOTING
        EnemyShooter shooter =
            GetComponent<EnemyShooter>();

        if (shooter != null)
        {
            shooter.enabled = false;
        }

        // DISABLE COLLIDERS
        Collider[] colliders =
            GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // RESET MOVEMENT ANIMATION
        if (anim != null)
        {
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", 0);
            anim.SetFloat("Speed", 0);

            anim.SetBool("Fire", false);

            // PLAY DEATH
            anim.SetBool("IsDead", true);
        }

        Destroy(gameObject, 4f);
    }
}