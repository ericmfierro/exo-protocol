using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 25f;
    [SerializeField] float fireRate = 10f; // shots per second
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject tracerPrefab;
    [SerializeField] Transform firePoint;

    StarterAssetsInputs starterAssetsInputs;
    float nextFireTime;

    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        // Hold to fire
        if (starterAssetsInputs.shoot && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity))
        {
            // Spawn hit particle
            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // Spawn tracer if available
            if (tracerPrefab != null && firePoint != null)
            {
                GameObject tracerObj = Instantiate(tracerPrefab);
                Tracer tracer = tracerObj.GetComponent<Tracer>();
                if (tracer != null)
                {
                    tracer.Setup(firePoint.position, hit.point);
                }
            }

            // Damage robot if hit
            Robot robot = hit.collider.GetComponent<Robot>();
            if (robot != null)
            {
                robot.TakeDamage(damage);
                return;
            }

            // Damage NPC if hit
            NPCController npc = hit.collider.GetComponent<NPCController>();
            if (npc != null)
            {
                npc.TakeDamage(damage);
            }
        }
    }
}