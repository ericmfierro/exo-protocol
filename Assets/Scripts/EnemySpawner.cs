using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public int enemiesPerWave = 3;
    public float timeBetweenSpawns = 0.5f;
    public float timeBetweenWaves = 8f;
    public int extraEnemiesPerWave = 1;
    public int maxEnemiesPerWave = 10;
    public float spawnRange = 50f;       // only spawn if player is within range
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
            // Only spawn if player is within range of this spawner
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
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(prefab, point.position, point.rotation);
        enemiesAlive++;

        EnemyDeathTracker tracker = enemy.AddComponent<EnemyDeathTracker>();
        tracker.spawner = this;
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;
    }
}

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