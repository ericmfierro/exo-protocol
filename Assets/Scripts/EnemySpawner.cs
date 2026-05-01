using UnityEngine;
using System.Collections;

// Wave based enemy spawner
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] GameObject[] enemyPrefabs;     // supports multiple enemy types
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int enemiesPerWave = 4;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float timeBetweenWaves = 5f;

    [Header("Scaling")]
    [SerializeField] int extraEnemiesPerWave = 1;
    [SerializeField] int maxEnemiesPerWave = 15;

    public int CurrentWave { get; private set; }
    int enemiesAlive;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        // Small delay so LevelManager initializes first
        yield return new WaitForSeconds(1f);

        while (true)
        {
            CurrentWave++;
            int count = Mathf.Min(
                enemiesPerWave + (CurrentWave - 1) * extraEnemiesPerWave,
                maxEnemiesPerWave);

            Debug.Log($"=== WAVE {CurrentWave} === Spawning {count} enemies");

            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            // Wait for all enemies to die
            while (enemiesAlive > 0)
            {
                yield return null;
            }

            Debug.Log($"Wave {CurrentWave} cleared!");
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        // Pick a random prefab and spawn point
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(prefab, point.position, point.rotation);
        enemiesAlive++;

        // Track death so we know when the wave is cleared
        Robot robot = enemy.GetComponent<Robot>();
        if (robot != null)
        {
            // Need to track when robot dies
            EnemyDeathTracker tracker = enemy.AddComponent<EnemyDeathTracker>();
            tracker.spawner = this;
        }
    }
    public void OnEnemyDied()
    {
        enemiesAlive--;
    }
}

// Helper that notifies the spawner when this enemy is killed
public class EnemyDeathTracker : MonoBehaviour
{
    [HideInInspector] public EnemySpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDied();
        }
    }
}
