using UnityEngine;

public class EnemyFootsteps : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource footstepSource;

    [Header("Footstep Clips")]
    public AudioClip walkClip;
    public AudioClip runClip;

    [Header("Volume")]
    public float walkVolume = 0.4f;
    public float runVolume = 0.7f;

    // WALK animation event
    public void PlayWalkFootstep()
    {
        if (walkClip == null ||
            footstepSource == null)
        {
            return;
        }

        footstepSource.pitch =
            Random.Range(0.95f, 1.05f);

        footstepSource.PlayOneShot(
            walkClip,
            walkVolume
        );
    }

    // RUN animation event
    public void PlayRunFootstep()
    {
        if (runClip == null ||
            footstepSource == null)
        {
            return;
        }

        footstepSource.pitch =
            Random.Range(0.95f, 1.05f);

        footstepSource.PlayOneShot(
            runClip,
            runVolume
        );
    }
}