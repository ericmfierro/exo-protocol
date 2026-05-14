using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] SanitySystem sanitySystem;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider sanityBar;
    [SerializeField] TextMeshProUGUI healthLabel;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI sanityLabel;

    [Header("Health Bar Colors")]
    [SerializeField] Color healthyColor = new Color(0f, 1f, 0.53f);
    [SerializeField] Color hurtColor = new Color(1f, 0.8f, 0f);
    [SerializeField] Color criticalColor = new Color(1f, 0.1f, 0.1f);

    [Header("Sanity Bar Colors")]
    [SerializeField] Color stableSanityColor = new Color(0.25f, 0.9f, 1f);
    [SerializeField] Color stressedSanityColor = new Color(0.7f, 0.35f, 1f);
    [SerializeField] Color criticalSanityColor = new Color(1f, 0.15f, 0.45f);

    Image fillImage;
    Image sanityFillImage;

    void Start()
    {
        if (healthBar != null)
        {
            fillImage = healthBar.fillRect.GetComponent<Image>();
        }

        if (sanityBar != null)
        {
            sanityFillImage = sanityBar.fillRect.GetComponent<Image>();
        }

        if (playerStats != null)
        {
            if (healthBar != null)
            {
                healthBar.maxValue = playerStats.maxHealth;
                healthBar.value = playerStats.maxHealth;
            }

            UpdateAmmo(playerStats.currentAmmo, playerStats.maxAmmo);

            // event listeners
            playerStats.OnHealthChanged.AddListener(UpdateHealth);
            playerStats.OnAmmoChanged.AddListener(UpdateAmmo);
        }

        if (sanitySystem == null)
        {
            sanitySystem = FindFirstObjectByType<SanitySystem>();
        }

        if (sanitySystem != null)
        {
            if (sanityBar != null)
            {
                sanityBar.maxValue = sanitySystem.maxSanity;
            }

            UpdateSanity(
                sanitySystem.currentSanity,
                sanitySystem.maxSanity
            );

            sanitySystem.OnSanityChanged?.AddListener(UpdateSanity);
        }
    }

    void OnDestroy()
    {
        if (sanitySystem != null)
        {
            sanitySystem.OnSanityChanged?.RemoveListener(UpdateSanity);
        }
    }

    void UpdateHealth(float current, float max)
    {
        if (healthBar != null)
        {
            healthBar.value = current;
        }

        if (healthLabel != null)
        {
            healthLabel.text = $"HP {Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }

        // Color based on health percentage
        if (fillImage != null)
        {
            float percent = current / max;
            if (percent > 0.5f) fillImage.color = healthyColor;
            else if (percent > 0.25f) fillImage.color = hurtColor;
            else fillImage.color = criticalColor;
        }
    }

    void UpdateAmmo(int current, int max)
    {
        if (ammoText != null)
        {
            ammoText.text = $"AMMO {current} / {max}";
        }
    }

    void UpdateSanity(float current, float max)
    {
        if (sanityBar != null)
        {
            sanityBar.maxValue = max;
            sanityBar.value = current;
        }

        if (sanityLabel != null)
        {
            sanityLabel.text = $"SANITY {Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }

        if (sanityFillImage != null)
        {
            float percent = current / max;
            if (percent > 0.5f) sanityFillImage.color = stableSanityColor;
            else if (percent > 0.25f) sanityFillImage.color = stressedSanityColor;
            else sanityFillImage.color = criticalSanityColor;
        }
    }
}
