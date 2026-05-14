using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] float damage = 25f;
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] int ammoPerShot = 1;
    [SerializeField] PlayerStats playerStats;

    [Header("Effects")]
    [SerializeField] GameObject hitEffect;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("Audio")]
    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip firingClip;

    [Header("Recoil")]
    [SerializeField] Transform weaponTransform;

    [SerializeField] float recoilKickback = 0.06f;
    [SerializeField] float recoilRotation = 2.5f;
    [SerializeField] float recoilRecoverySpeed = 10f;

    float nextTimeToFire = 0f;

    bool wasShootingLastFrame = false;

    Vector3 originalPosition;
    Quaternion originalRotation;

    void Start()
    {
        if (playerStats == null)
        {
            playerStats = GetComponentInParent<PlayerStats>();
        }

        if (playerStats == null)
        {
            playerStats = FindFirstObjectByType<PlayerStats>();
        }

        originalPosition =
            weaponTransform.localPosition;

        originalRotation =
            weaponTransform.localRotation;
    }

    void Update()
    {
        // SMOOTH RECOVERY
        weaponTransform.localPosition =
            Vector3.Lerp(
                weaponTransform.localPosition,
                originalPosition,
                Time.deltaTime *
                recoilRecoverySpeed
            );

        weaponTransform.localRotation =
            Quaternion.Lerp(
                weaponTransform.localRotation,
                originalRotation,
                Time.deltaTime *
                recoilRecoverySpeed
            );

        // HOLD LEFT CLICK FOR FULL AUTO
        if (Input.GetMouseButton(0))
        {
            bool triedToFire = false;
            bool firedThisFrame = false;

            if (Time.time >= nextTimeToFire)
            {
                triedToFire = true;
                nextTimeToFire =
                    Time.time + fireRate;

                firedThisFrame = Shoot();
            }

            // START AUDIO ONLY ONCE
            if (firedThisFrame &&
                !wasShootingLastFrame)
            {
                if (gunAudioSource != null &&
                    firingClip != null)
                {
                    gunAudioSource.clip =
                        firingClip;

                    gunAudioSource.loop = true;

                    gunAudioSource.Play();
                }

                wasShootingLastFrame = true;
            }
            else if (triedToFire &&
                     !firedThisFrame &&
                     wasShootingLastFrame)
            {
                StopFiringEffects();
            }
        }
        else
        {
            // STOP AUDIO WHEN FIRE RELEASED
            StopFiringEffects();
        }
    }

    bool Shoot()
    {
        if (playerStats != null &&
            !playerStats.UseAmmo(ammoPerShot))
        {
            return false;
        }

        // PLAY MUZZLE FLASH
        if (muzzleFlash != null)
        {
            muzzleFlash.Stop(
                true,
                ParticleSystemStopBehavior
                    .StopEmittingAndClear
            );

            muzzleFlash.Play();
        }

        // RECOIL POSITION
        weaponTransform.localPosition -=
            new Vector3(
                0f,
                0f,
                recoilKickback
            );

        // RECOIL ROTATION
        weaponTransform.localRotation *=
            Quaternion.Euler(
                -recoilRotation,
                Random.Range(-0.5f, 0.5f),
                0f
            );

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
                    Quaternion.LookRotation(
                        hit.normal
                    )
                );
            }

            // ROBOT DAMAGE
            Robot robot =
                hit.collider.GetComponent<Robot>();

            if (robot != null)
            {
                robot.TakeDamage(damage);
                return true;
            }

            // HEALTH DAMAGE
            Health health =
                hit.collider.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        return true;
    }

    void StopFiringEffects()
    {
        if (wasShootingLastFrame)
        {
            if (gunAudioSource != null)
            {
                gunAudioSource.Stop();
            }

            wasShootingLastFrame = false;
        }

        // STOP MUZZLE FLASH
        if (muzzleFlash != null)
        {
            muzzleFlash.Stop(
                true,
                ParticleSystemStopBehavior
                    .StopEmittingAndClear
            );
        }
    }
}
