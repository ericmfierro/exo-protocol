using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] float damage = 25f;

    // Lower = faster automatic fire
    [SerializeField] float fireRate = 0.1f;

    [Header("Effects")]
    [SerializeField] GameObject hitEffect;

    [Header("Audio")]
    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip firingClip;

    StarterAssetsInputs starterAssetsInputs;

    float nextTimeToFire = 0f;

    void Awake()
    {
        starterAssetsInputs =
            GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        // HOLD LEFT MOUSE FOR FULL AUTO
        if (Input.GetMouseButton(0))
        {
            // START FIRING AUDIO
            if (gunAudioSource != null &&
                firingClip != null)
            {
                if (!gunAudioSource.isPlaying)
                {
                    gunAudioSource.clip =
                        firingClip;

                    gunAudioSource.loop = true;

                    gunAudioSource.pitch =
                        Random.Range(0.98f, 1.02f);

                    gunAudioSource.Play();
                }
            }

            // FIRE RATE CONTROL
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire =
                    Time.time + fireRate;

                Shoot();
            }
        }
        else
        {
            // STOP AUDIO IMMEDIATELY
            if (gunAudioSource != null &&
                gunAudioSource.isPlaying)
            {
                gunAudioSource.Stop();
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            Mathf.Infinity))
        {
            // HIT EFFECT
            if (hitEffect != null)
            {
                Instantiate(
                    hitEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );
            }

            // ROBOT DAMAGE
            Robot robot =
                hit.collider.GetComponent<Robot>();

            if (robot != null)
            {
                robot.TakeDamage(damage);
                return;
            }

            // GENERIC HEALTH DAMAGE
            Health health =
                hit.collider.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}