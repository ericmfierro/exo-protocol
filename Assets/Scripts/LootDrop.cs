using UnityEngine;

// Spawns a pickup when this enemy dies
public class LootDrop : MonoBehaviour
{
    [SerializeField] GameObject[] lootTable;
    [SerializeField] float dropChance = 0.6f;
    [SerializeField] float spawnHeightOffset = 0.5f;

    Vector3 lastPosition;

    void Update()
    {
        lastPosition = transform.position;
    }

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;

        if (lootTable.Length == 0) return;
        if (Random.value > dropChance) return;

        GameObject lootPrefab = lootTable[Random.Range(0, lootTable.Length)];
        Vector3 spawnPos = lastPosition + Vector3.up * spawnHeightOffset;

        Instantiate(lootPrefab, spawnPos, Quaternion.identity);
    }
}
