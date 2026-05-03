using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Health Bar")]
    [SerializeField] Slider healthSlider;

    [Header("Optional Debug")]
    public bool showDebugLogs = true;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // Called when robot takes damage
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (showDebugLogs)
        {
            Debug.Log(name + " took damage. Health: " + currentHealth);
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Get current health
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // Get health percentage 
    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    // Death logic
    void Die()
    {
        if (showDebugLogs)
        {
            Debug.Log(name + " died.");
        }

        // Trigger kill chain system
        if (KillChainManager.Instance != null)
        {
            KillChainManager.Instance.RegisterKill();
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.EnemyKilled();
        }

        Destroy(gameObject);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}