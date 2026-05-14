using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth = 100f;

    [Header("UI")]
    [SerializeField] Slider healthSlider;

    [Header("Debug")]
    [SerializeField] bool invincible = true;

    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue =
                maxHealth;

            healthSlider.value =
                currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        // TEMP INVINCIBILITY
        if (invincible)
        {
            Debug.Log(
                gameObject.name +
                " is invincible."
            );

            return;
        }

        currentHealth -= amount;

        Debug.Log(
            gameObject.name +
            " health: " +
            currentHealth
        );

        if (healthSlider != null)
        {
            healthSlider.value =
                currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}