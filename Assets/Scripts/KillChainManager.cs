using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


// Added
// Score tracking with multiplier
// Multiplier that scales with chain length
// UnityEvents so the HUD can see
// ChainTimeRemaining for timer bar UI
public class KillChainManager : MonoBehaviour
{
    public static KillChainManager Instance;

    [Header("Combo Settings")]
    public float comboWindow = 3f;

    [Header("Rewards")]
    public float baseHealthGain = 10f;
    public int baseAmmoGain = 5;

    [Header("Score")]
    [SerializeField] int baseScorePerKill = 100;
    [SerializeField] int maxMultiplier = 8;

    [Header("Win Condition")]
    [SerializeField] int killsToWin = 10;
    [SerializeField] int wonSceneIndex = 3;

    // States
    public int ComboCount { get; private set; }
    public int Multiplier { get; private set; } = 1;
    public float ChainTimeRemaining { get; private set; }
    public int TotalScore { get; private set; }
    public int TotalKills { get; private set; }

    //  UI events
    public UnityEvent<int> OnChainUpdated;        // passes combo count
    public UnityEvent<int> OnMultiplierChanged;   // passes multiplier
    public UnityEvent<int> OnScoreChanged;        // passes total score
    public UnityEvent OnChainBroken;

    private float comboTimer = 0f;
    private bool chainActive;
    private bool winStarted;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!chainActive) return;

        comboTimer -= Time.deltaTime;
        ChainTimeRemaining = Mathf.Max(0f, comboTimer);

        if (comboTimer <= 0f)
        {
            ResetCombo();
        }
    }

    // CALL THIS WHEN ENEMY DIES
    public void RegisterKill()
    {
        if (winStarted) return;

        TotalKills++;
        ComboCount++;
        comboTimer = comboWindow;
        chainActive = true;

        // Multiplier doubles at chains 2, 4, 8
        Multiplier = Mathf.Min(1 << Mathf.FloorToInt(Mathf.Log(Mathf.Max(ComboCount, 1), 2)), maxMultiplier);

        // Score
        int killScore = baseScorePerKill * Multiplier;
        TotalScore += killScore;

        // Rewards 
        float healthGained = baseHealthGain * Multiplier;
        int ammoGained = baseAmmoGain * Multiplier;
        GiveRewards(healthGained, ammoGained);

        // Fire events for HUD
        OnChainUpdated?.Invoke(ComboCount);
        OnMultiplierChanged?.Invoke(Multiplier);
        OnScoreChanged?.Invoke(TotalScore);

        Debug.Log($"CHAIN x{ComboCount} | x{Multiplier} MULT | +{healthGained} HP | +{ammoGained} Ammo | Score: {TotalScore}");

        if (TotalKills >= killsToWin)
        {
            LoadWinScene();
        }
    }

    void LoadWinScene()
    {
        if (winStarted) return;

        winStarted = true;
        SceneManager.LoadScene(wonSceneIndex);
    }

    void ResetCombo()
    {
        if (ComboCount > 0)
        {
            Debug.Log("Chain ended at: " + ComboCount);
        }

        chainActive = false;
        ComboCount = 0;
        Multiplier = 1;
        ChainTimeRemaining = 0f;

        OnChainBroken?.Invoke();
        OnChainUpdated?.Invoke(0);
        OnMultiplierChanged?.Invoke(1);
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
