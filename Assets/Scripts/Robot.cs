using UnityEngine;
using UnityEngine.UI;

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

    // IMPORTANT:
    // Needed by EliteAI
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

        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }

        Destroy(gameObject, 3f);
    }
}