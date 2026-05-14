using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 25f;
    [SerializeField] float fireRate = 10f; // shots per second
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject tracerPrefab;
    [SerializeField] Transform firePoint;

    [Header("Effects")]
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("Audio")]
    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip firingClip;

    [Header("Recoil")]
    [SerializeField] Transform weaponTransform;
    [SerializeField] float recoilKickback = 0.06f;
    [SerializeField] float recoilRotation = 2.5f;
    [SerializeField] float recoilRecoverySpeed = 10f;

    StarterAssetsInputs starterAssetsInputs;
    float nextFireTime;
    bool wasShootingLastFrame;
    Vector3 originalPosition;
    Quaternion originalRotation;

    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();

        if (weaponTransform != null)
        {
            originalPosition = weaponTransform.localPosition;
            originalRotation = weaponTransform.localRotation;
        }
    }

    void Update()
    {
        // Smooth recoil recovery
        if (weaponTransform != null)
        {
            weaponTransform.localPosition = Vector3.Lerp(
                weaponTransform.localPosition,
                originalPosition,
                Time.deltaTime * recoilRecoverySpeed);

            weaponTransform.localRotation = Quaternion.Lerp(
                weaponTransform.localRotation,
                originalRotation,
                Time.deltaTime * recoilRecoverySpeed);
        }

        // Hold to fire
        if (starterAssetsInputs.shoot)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + (1f / fireRate);
            }

            // Start audio loop on first frame of firing
            if (!wasShootingLastFrame)
            {
                if (gunAudioSource != null && firingClip != null)
                {
                    gunAudioSource.clip = firingClip;
                    gunAudioSource.loop = true;
                    gunAudioSource.Play();
                }
                wasShootingLastFrame = true;
            }
        }
        else
        {
            // Stop audio when fire released
            if (wasShootingLastFrame)
            {
                if (gunAudioSource != null)
                {
                    gunAudioSource.Stop();
                }
                wasShootingLastFrame = false;
            }

            // Stop muzzle flash
            if (muzzleFlash != null)
            {
                muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    void Shoot()
    {
        // Muzzle flash
        if (muzzleFlash != null && !muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        // Apply recoil
        if (weaponTransform != null)
        {
            weaponTransform.localPosition -= new Vector3(0, 0, recoilKickback);
            weaponTransform.localRotation *= Quaternion.Euler(-recoilRotation, 0, 0);
        }

        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity))
        {
            // Spawn hit particle
            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // Spawn tracer
            if (tracerPrefab != null && firePoint != null)
            {
                GameObject tracerObj = Instantiate(tracerPrefab);
                Tracer tracer = tracerObj.GetComponent<Tracer>();
                if (tracer != null)
                {
                    tracer.Setup(firePoint.position, hit.point);
                }
            }

            // Damage robot
            Robot robot = hit.collider.GetComponent<Robot>();
            if (robot != null)
            {
                robot.TakeDamage(damage);
                return;
            }

            // Damage NPC
            NPCController npc = hit.collider.GetComponent<NPCController>();
            if (npc != null)
            {
                npc.TakeDamage(damage);
                return;
            }

            // Fallback - damage anything with Health component
            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}