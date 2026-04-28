using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;

public class BulletTimeController : MonoBehaviour
{
    [Header("Bullet Time Settings")]
    public float slowTimeScale = 0.3f;
    public float normalTimeScale = 1f;

    public float maxDuration = 3f;
    private float currentDuration;

    public float cooldownTime = 5f;
    private float cooldownTimer = 0f;

    [Header("Player Boost")]
    public float speedMultiplier = 2f;
    public float rotationMultiplier = 1.5f;

    private bool isActive = false;

    private FirstPersonController player;

    void Start()
    {
        player = FindFirstObjectByType<FirstPersonController>();
    }

    void Update()
    {
        HandleInput();
        HandleTimers();
    }

    void HandleInput()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && cooldownTimer <= 0f && !isActive)
        {
            ActivateBulletTime();
        }
    }

    void HandleTimers()
    {
        if (isActive)
        {
            currentDuration -= Time.unscaledDeltaTime;

            if (currentDuration <= 0f)
            {
                DeactivateBulletTime();
            }
        }
        else
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    void ActivateBulletTime()
    {
        isActive = true;
        currentDuration = maxDuration;

        // Slow world
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Boost player so they feel powerful
        if (player != null)
        {
            player.MoveSpeed *= speedMultiplier;
            player.RotationSpeed *= rotationMultiplier;
        }

        Debug.Log("Bullet Time ON");
    }

    void DeactivateBulletTime()
    {
        if (!isActive) return;

        isActive = false;

        // Reset time
        Time.timeScale = normalTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Reset player stats
        if (player != null)
        {
            player.MoveSpeed /= speedMultiplier;
            player.RotationSpeed /= rotationMultiplier;
        }

        cooldownTimer = cooldownTime;

        Debug.Log("Bullet Time OFF");
    }
}