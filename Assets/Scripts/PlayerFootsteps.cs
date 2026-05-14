using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource footstepSource;

    public AudioClip walkingClip;

    public CharacterController controller;

    public float movementThreshold = 0.1f;

    void Update()
    {
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        bool isMoving = horizontalVelocity.magnitude > movementThreshold;

        if (isMoving)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.clip = walkingClip;
                footstepSource.loop = true;
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
    }
}