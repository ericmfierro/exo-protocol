using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanity")]
    public float maxSanity = 100f;
    public float currentSanity = 100f;

    public UnityEvent<float, float> OnSanityChanged =
        new UnityEvent<float, float>(); // current, max

    [Header("Passive Drain")]
    public float passiveDrainRate = 1f;

    [Header("Psychosis Audio Sources")]
    public AudioSource psychosisSourceA;
    public AudioSource psychosisSourceB;

    [Header("Level 1 Audio (70% and below)")]
    public AudioClip[] level1Clips;

    [Header("Level 2 Audio (40% and below)")]
    public AudioClip[] level2Clips;

    [Header("Level 3 Audio (15% and below)")]
    public AudioClip[] level3Clips;

    float nextPsychosisTime = 0f;

    void Start()
    {
        currentSanity =
            Mathf.Clamp(
                currentSanity,
                0f,
                maxSanity
            );

        OnSanityChanged?.Invoke(
            currentSanity,
            maxSanity
        );

        // IMMEDIATE FIRST EVENT CHECK
        nextPsychosisTime = 0f;
    }

    void Update()
    {
        float previousSanity =
            currentSanity;

        // PASSIVE SANITY DRAIN
        currentSanity -=
            passiveDrainRate *
            Time.deltaTime;

        currentSanity =
            Mathf.Clamp(
                currentSanity,
                0f,
                maxSanity
            );

        if (!Mathf.Approximately(
                previousSanity,
                currentSanity))
        {
            OnSanityChanged?.Invoke(
                currentSanity,
                maxSanity
            );
        }

        HandlePsychosis();

        // OPTIONAL DEBUG
        // Debug.Log(currentSanity);
    }

    void HandlePsychosis()
    {
        // WAIT FOR NEXT EVENT
        if (Time.time < nextPsychosisTime)
            return;

        float sanityPercent =
            currentSanity / maxSanity;

        // ACTIVE AUDIO POOL
        List<AudioClip> activeClips =
            new List<AudioClip>();

        // LEVEL 1
        if (sanityPercent <= 0.7f)
        {
            activeClips.AddRange(level1Clips);
        }

        // LEVEL 2
        if (sanityPercent <= 0.4f)
        {
            activeClips.AddRange(level2Clips);
        }

        // LEVEL 3
        if (sanityPercent <= 0.15f)
        {
            activeClips.AddRange(level3Clips);
        }

        // PLAY RANDOM ACTIVE CLIP
        if (activeClips.Count > 0)
        {
            AudioClip selectedClip =
                activeClips[
                    Random.Range(
                        0,
                        activeClips.Count
                    )
                ];

            PlayClip(selectedClip);
        }

        // EVENT FREQUENCY
        if (sanityPercent > 0.7f)
        {
            nextPsychosisTime =
                Time.time +
                Random.Range(20f, 30f);
        }
        else if (sanityPercent > 0.4f)
        {
            nextPsychosisTime =
                Time.time +
                Random.Range(10f, 18f);
        }
        else if (sanityPercent > 0.15f)
        {
            nextPsychosisTime =
                Time.time +
                Random.Range(5f, 10f);
        }
        else
        {
            // VERY FREQUENT AT CRITICAL SANITY
            nextPsychosisTime =
                Time.time +
                Random.Range(1f, 3f);
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (clip == null)
            return;

        AudioSource selectedSource = null;

        // USE FIRST SOURCE IF AVAILABLE
        if (psychosisSourceA != null &&
            !psychosisSourceA.isPlaying)
        {
            selectedSource =
                psychosisSourceA;
        }

        // OTHERWISE USE SECOND SOURCE
        else if (psychosisSourceB != null &&
                 !psychosisSourceB.isPlaying)
        {
            selectedSource =
                psychosisSourceB;
        }

        // BOTH SOURCES BUSY
        if (selectedSource == null)
            return;

        // SLIGHT VARIATION
        selectedSource.pitch =
            Random.Range(0.95f, 1.05f);

        Debug.Log(
            "Playing Psychosis Audio: " +
            clip.name
        );

        selectedSource.PlayOneShot(clip);
    }

    // REDUCE SANITY
    public void ReduceSanity(float amount)
    {
        SetSanity(currentSanity - amount);
    }

    // RESTORE SANITY
    public void RestoreSanity(float amount)
    {
        SetSanity(currentSanity + amount);
    }

    void SetSanity(float value)
    {
        currentSanity =
            Mathf.Clamp(
                value,
                0f,
                maxSanity
            );

        OnSanityChanged?.Invoke(
            currentSanity,
            maxSanity
        );
    }
}
