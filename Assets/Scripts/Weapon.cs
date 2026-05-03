using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 25f;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] GameObject hitEffect;

    StarterAssetsInputs starterAssetsInputs;
    float nextTimeToFire = 0f;

    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (starterAssetsInputs.shoot)
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }

            starterAssetsInputs.ShootInput(false);
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
            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            Robot robot = hit.collider.GetComponent<Robot>();
            if (robot != null)
            {
                robot.TakeDamage(damage);
                return;
            }

            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
