using UnityEngine;

public class KillChainManager : MonoBehaviour
{
    public static KillChainManager Instance;

    [Header("Combo Settings")]
    public float comboWindow = 3f;

    private int comboCount = 0;
    private float comboTimer = 0f;

    [Header("Rewards")]
    public float baseHealthGain = 10f;
    public int baseAmmoGain = 5;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            ResetCombo();
        }
    }

    // CALL THIS WHEN ENEMY DIES
    public void RegisterKill()
    {
        comboCount++;
        comboTimer = comboWindow;

        float healthGained = baseHealthGain * comboCount;
        int ammoGained = baseAmmoGain * comboCount;

        Debug.Log("Combo: " + comboCount +
                  " | +Health: " + healthGained +
                  " | +Ammo: " + ammoGained);

        GiveRewards(healthGained, ammoGained);
    }

    void ResetCombo()
    {
        if (comboCount > 0)
        {
            Debug.Log("Combo Ended at: " + comboCount);
        }

        comboCount = 0;
    }

    void GiveRewards(float health, int ammo)
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();

        if (player != null)
        {
            player.AddHealth(health);
            player.AddAmmo(ammo);
        }
    }
}