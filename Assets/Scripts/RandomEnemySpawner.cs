using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RandomEnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnRadius = 30f;
    public float minDistanceFromPlayer = 15f;
    public int enemiesPerWave = 3;
    public float timeBetweenSpawns = 0.5f;
    public float timeBetweenWaves = 8f;
    public int extraEnemiesPerWave = 1;
    public int maxEnemiesPerWave = 10;
    public float spawnRange = 50f;
    public bool spawnInfinitely = true;

    public int CurrentWave { get; private set; }
    int enemiesAlive;
    Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(1f);

        while (spawnInfinitely)
        {
            if (player != null && Vector3.Distance(transform.position, player.position) > spawnRange)
            {
                yield return new WaitForSeconds(2f);
                continue;
            }

            CurrentWave++;
            int count = Mathf.Min(
                enemiesPerWave + (CurrentWave - 1) * extraEnemiesPerWave,
                maxEnemiesPerWave);

            Debug.Log(name + " Wave " + CurrentWave + " spawning " + count);

            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            while (enemiesAlive > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        Vector3 spawnPos = GetRandomSpawnPoint();

        if (spawnPos == Vector3.zero) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        enemiesAlive++;

        RandomSpawnerDeathTracker tracker = enemy.AddComponent<RandomSpawnerDeathTracker>();
        tracker.spawner = this;
    }

    Vector3 GetRandomSpawnPoint()
    {
        // Try up to 30 times to find a valid spawn point
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * spawnRadius;
            randomDir += transform.position;
            randomDir.y = transform.position.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, spawnRadius, NavMesh.AllAreas))
            {
                // Make sure it's far enough from the player
                if (player == null || Vector3.Distance(hit.position, player.position) >= minDistanceFromPlayer)
                {
                    return hit.position;
                }
            }
        }

        return Vector3.zero;
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;
    }

    // Show the spawn radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromPlayer);
    }
}

public class RandomSpawnerDeathTracker : MonoBehaviour
{
    [HideInInspector] public RandomEnemySpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDied();
        }
    }
}