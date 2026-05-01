using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public int maxAmmo = 100;
    public int currentAmmo;

    public UnityEvent<float, float> OnHealthChanged;  // current, max
    public UnityEvent<int, int> OnAmmoChanged;         // current, max
    public UnityEvent OnPlayerDeath;

    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Health: " + currentHealth);
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        Debug.Log("Ammo: " + currentAmmo);
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
            Debug.Log("PLAYER DOWN");
        }
    }

    public bool UseAmmo(int amount)
    {
        if (currentAmmo <= 0) return false;

        currentAmmo = Mathf.Max(currentAmmo - amount, 0);
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        return true;
    }
}
