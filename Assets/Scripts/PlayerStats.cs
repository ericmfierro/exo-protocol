using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 2000f;
    public float currentHealth;

    public int maxAmmo = 100;
    public int currentAmmo;

    [Header("Scenes")]
    [SerializeField] int gameOverSceneIndex = 2;

    public UnityEvent<float, float> OnHealthChanged;  // current, max
    public UnityEvent<int, int> OnAmmoChanged;         // current, max
    public UnityEvent OnPlayerDeath;

    bool gameOverStarted;

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
            LoadGameOver();
        }
    }

    public bool UseAmmo(int amount)
    {
        if (amount <= 0) return true;
        if (currentAmmo < amount)
        {
            if (currentAmmo <= 0)
            {
                StartCoroutine(LoadGameOverAfterShot());
            }

            return false;
        }

        currentAmmo -= amount;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);

        if (currentAmmo <= 0)
        {
            Debug.Log("OUT OF AMMO");
            StartCoroutine(LoadGameOverAfterShot());
        }

        return true;
    }

    IEnumerator LoadGameOverAfterShot()
    {
        yield return null;

        if (currentAmmo <= 0)
        {
            LoadGameOver();
        }
    }

    void LoadGameOver()
    {
        if (gameOverStarted) return;

        gameOverStarted = true;
        SceneManager.LoadScene(gameOverSceneIndex);
    }
}
