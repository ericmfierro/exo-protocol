using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SanityVisualEffects : MonoBehaviour
{
    [Header("References")]
    public SanitySystem sanitySystem;
    public Camera playerCamera;
    public Volume globalVolume;

    [Header("FOV")]
    public float normalFOV = 60f;
    public float panicFOV = 64f;

    ChromaticAberration chromatic;
    Vignette vignette;
    FilmGrain filmGrain;
    ColorAdjustments colorAdjustments;
    DepthOfField depthOfField;
    MotionBlur motionBlur;

    bool pulseActive = false;

    void Start()
    {
        globalVolume.profile.TryGet(
            out chromatic);

        globalVolume.profile.TryGet(
            out vignette);

        globalVolume.profile.TryGet(
            out filmGrain);

        globalVolume.profile.TryGet(
            out colorAdjustments);

        globalVolume.profile.TryGet(
            out depthOfField);

        globalVolume.profile.TryGet(
            out motionBlur);

        ResetEffects();

        StartCoroutine(
            PulseLoop()
        );
    }

    IEnumerator PulseLoop()
    {
        while (true)
        {
            float sanityPercent =
                sanitySystem.currentSanity /
                sanitySystem.maxSanity;

            float insanity =
                1f - sanityPercent;

            // MORE INSANITY =
            // MORE FREQUENT PULSES
            float delay =
                Mathf.Lerp(
                    45f,
                    6f,
                    insanity
                );

            yield return new WaitForSeconds(
                Random.Range(
                    delay * 0.7f,
                    delay
                )
            );

            if (!pulseActive)
            {
                StartCoroutine(
                    PanicPulse(insanity)
                );
            }
        }
    }

    IEnumerator PanicPulse(float insanity)
    {
        pulseActive = true;

        float duration =
            Mathf.Lerp(
                0.6f,
                2f,
                insanity
            );

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float pulse =
                Mathf.Sin(
                    elapsed * 8f
                );

            float t =
                (pulse + 1f) * 0.5f;

            // FOV BREATHING
            playerCamera.fieldOfView =
                Mathf.Lerp(
                    normalFOV,
                    panicFOV,
                    t * insanity
                );

            // CHROMATIC SPIKES
            chromatic.intensity.value =
                Mathf.Lerp(
                    0f,
                    0.35f,
                    t * insanity
                );

            // BREATHING DARK/LIGHT
            colorAdjustments.postExposure.value =
                Mathf.Lerp(
                    -0.25f,
                    0.05f,
                    t
                );

            // DESATURATION
            colorAdjustments.saturation.value =
                Mathf.Lerp(
                    0f,
                    -30f,
                    insanity
                );

            // RED/GREY PULSE
            Color baseColor =
                Color.Lerp(
                    Color.white,
                    new Color(
                        0.35f,
                        0.3f,
                        0.3f
                    ),
                    insanity * 0.5f
                );

            Color pulseColor =
                Color.Lerp(
                    baseColor,
                    new Color(
                        0.45f,
                        0.18f,
                        0.18f
                    ),
                    t * 0.35f
                );

            colorAdjustments.colorFilter.value =
                pulseColor;

            // BLUR PULSE
            depthOfField.gaussianMaxRadius.value =
                Mathf.Lerp(
                    0f,
                    0.2f,
                    t * insanity
                );

            // LIGHT MOTION BLUR
            motionBlur.intensity.value =
                Mathf.Lerp(
                    0f,
                    0.08f,
                    t * insanity
                );

            // VIGNETTE BREATHING
            vignette.intensity.value =
                Mathf.Lerp(
                    0f,
                    0.22f,
                    t * insanity
                );

            // FILM GRAIN
            filmGrain.intensity.value =
                Mathf.Lerp(
                    0f,
                    0.15f,
                    insanity
                );

            yield return null;
        }

        // SMOOTH RECOVERY
        float recovery = 0f;

        while (recovery < 1f)
        {
            recovery += Time.deltaTime * 1.5f;

            playerCamera.fieldOfView =
                Mathf.Lerp(
                    playerCamera.fieldOfView,
                    normalFOV,
                    recovery
                );

            chromatic.intensity.value =
                Mathf.Lerp(
                    chromatic.intensity.value,
                    0f,
                    recovery
                );

            depthOfField.gaussianMaxRadius.value =
                Mathf.Lerp(
                    depthOfField.gaussianMaxRadius.value,
                    0f,
                    recovery
                );

            motionBlur.intensity.value =
                Mathf.Lerp(
                    motionBlur.intensity.value,
                    0f,
                    recovery
                );

            vignette.intensity.value =
                Mathf.Lerp(
                    vignette.intensity.value,
                    0f,
                    recovery
                );

            colorAdjustments.postExposure.value =
                Mathf.Lerp(
                    colorAdjustments.postExposure.value,
                    0f,
                    recovery
                );

            colorAdjustments.colorFilter.value =
                Color.Lerp(
                    colorAdjustments.colorFilter.value,
                    Color.white,
                    recovery
                );

            yield return null;
        }

        ResetEffects();

        pulseActive = false;
    }

    void ResetEffects()
    {
        playerCamera.fieldOfView =
            normalFOV;

        chromatic.intensity.value = 0f;

        vignette.intensity.value = 0f;

        filmGrain.intensity.value = 0f;

        depthOfField.gaussianMaxRadius.value = 0f;

        motionBlur.intensity.value = 0f;

        colorAdjustments.postExposure.value = 0f;

        colorAdjustments.saturation.value = 0f;

        colorAdjustments.colorFilter.value =
            Color.white;
    }
}