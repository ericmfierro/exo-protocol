using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthLabel;
    [SerializeField] TextMeshProUGUI ammoText;

    [Header("Health Bar Colors")]
    [SerializeField] Color healthyColor = new Color(0f, 1f, 0.53f);
    [SerializeField] Color hurtColor = new Color(1f, 0.8f, 0f);
    [SerializeField] Color criticalColor = new Color(1f, 0.1f, 0.1f);

    Image fillImage;

    void Start()
    {
        if (healthBar != null)
        {
            fillImage = healthBar.fillRect.GetComponent<Image>();
        }

        if (playerStats != null)
        {
            healthBar.maxValue = playerStats.maxHealth;
            healthBar.value = playerStats.maxHealth;
            UpdateAmmo(playerStats.currentAmmo, playerStats.maxAmmo);

            // event listeners
            playerStats.OnHealthChanged.AddListener(UpdateHealth);
            playerStats.OnAmmoChanged.AddListener(UpdateAmmo);
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
}
