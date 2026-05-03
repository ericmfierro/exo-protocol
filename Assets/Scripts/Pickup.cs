using UnityEngine;


// Collectible pickup that triggers on player contact
public class Pickup : MonoBehaviour
{
    public enum PickupType { Health, Ammo, Score }

    [SerializeField] PickupType pickupType = PickupType.Health;
    [SerializeField] float healthAmount = 25f;
    [SerializeField] int ammoAmount = 15;
    [SerializeField] int scoreAmount = 50;
    [SerializeField] float lifetime = 10f;
    [SerializeField] float bobSpeed = 2f;
    [SerializeField] float bobHeight = 0.2f;
    [SerializeField] float rotateSpeed = 90f;

    Vector3 startPos;
    bool collected;

    void Start()
    {
        startPos = transform.position;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        // Make sure its on the player layer
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        collected = true;

        PlayerStats stats = other.GetComponentInParent<PlayerStats>();

        switch (pickupType)
        {
            case PickupType.Health:
                if (stats != null)
                {
                    stats.AddHealth(healthAmount);
                }
                Debug.Log($"+{healthAmount} HEALTH");
                break;

            case PickupType.Ammo:
                if (stats != null)
                {
                    stats.AddAmmo(ammoAmount);
                }
                Debug.Log($"+{ammoAmount} AMMO");
                break;

            case PickupType.Score:
                // Add to kill chain
                Debug.Log($"+{scoreAmount} SCORE BONUS");
                break;
        }

        Destroy(gameObject);
    }
}
