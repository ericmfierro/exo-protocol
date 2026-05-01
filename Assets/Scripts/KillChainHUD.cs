using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Displays kill chain info on the Overlay Canvas
public class KillChainHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI chainText;
    [SerializeField] TextMeshProUGUI multiplierText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider chainTimerBar;

    [Header("Style")]
    [SerializeField] Color normalColor = new Color(0f, 1f, 0.9f);
    [SerializeField] Color highChainColor = new Color(1f, 0.2f, 0.1f);
    [SerializeField] int highChainThreshold = 5;

    [Header("Animation")]
    [SerializeField] float punchScale = 1.3f;
    [SerializeField] float punchSpeed = 5f;

    float currentScale = 1f;
    bool isAnimating;

    void Start()
    {
        SetChainVisible(false);
        UpdateScore(0);
    }

    void OnEnable()
    {
        // Wait a frame for KillChainManager to initialize
        Invoke(nameof(Listen), 0.1f);
    }

    void Listen()
    {
        if (KillChainManager.Instance != null)
        {
            KillChainManager.Instance.OnChainUpdated.AddListener(UpdateChain);
            KillChainManager.Instance.OnMultiplierChanged.AddListener(UpdateMultiplier);
            KillChainManager.Instance.OnScoreChanged.AddListener(UpdateScore);
            KillChainManager.Instance.OnChainBroken.AddListener(OnChainBroken);
        }
    }

    void OnDisable()
    {
        if (KillChainManager.Instance != null)
        {
            KillChainManager.Instance.OnChainUpdated.RemoveListener(UpdateChain);
            KillChainManager.Instance.OnMultiplierChanged.RemoveListener(UpdateMultiplier);
            KillChainManager.Instance.OnScoreChanged.RemoveListener(UpdateScore);
            KillChainManager.Instance.OnChainBroken.RemoveListener(OnChainBroken);
        }
    }

    void Update()
    {
        // Update chain timer bar
        if (KillChainManager.Instance != null && chainTimerBar != null)
        {
            chainTimerBar.value = KillChainManager.Instance.ChainTimeRemaining;
        }

        // Text Animation
        if (isAnimating)
        {
            currentScale = Mathf.Lerp(currentScale, 1f, Time.unscaledDeltaTime * punchSpeed);
            if (chainText != null) chainText.transform.localScale = Vector3.one * currentScale;
            if (multiplierText != null) multiplierText.transform.localScale = Vector3.one * currentScale;

            if (Mathf.Abs(currentScale - 1f) < 0.01f)
            {
                currentScale = 1f;
                isAnimating = false;
            }
        }
    }

    void UpdateChain(int chain)
    {
        if (chain <= 0)
        {
            SetChainVisible(false);
            return;
        }

        SetChainVisible(true);
        chainText.text = $"KILL CHAIN x{chain}";
        chainText.color = chain >= highChainThreshold ? highChainColor : normalColor;

        if (chainTimerBar != null)
        {
            chainTimerBar.maxValue = KillChainManager.Instance.comboWindow;
            chainTimerBar.value = KillChainManager.Instance.comboWindow;
        }

        currentScale = punchScale;
        isAnimating = true;
    }

    void UpdateMultiplier(int multiplier)
    {
        if (multiplier <= 1)
        {
            if (multiplierText != null) multiplierText.gameObject.SetActive(false);
            return;
        }

        if (multiplierText != null)
        {
            multiplierText.gameObject.SetActive(true);
            multiplierText.text = $"x{multiplier} MULTIPLIER";
            multiplierText.color = multiplier >= 4 ? highChainColor : normalColor;
        }
    }

    void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {score:N0}";
        }
    }

    void OnChainBroken()
    {
        SetChainVisible(false);
    }

    void SetChainVisible(bool visible)
    {
        if (chainText != null) chainText.gameObject.SetActive(visible);
        if (multiplierText != null) multiplierText.gameObject.SetActive(visible);
        if (chainTimerBar != null) chainTimerBar.gameObject.SetActive(visible);
    }
}
