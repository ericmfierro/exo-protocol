using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private int enemiesRemaining;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemiesRemaining = FindObjectsByType<Robot>(FindObjectsSortMode.None).Length;

        Debug.Log("Enemies in level: " + enemiesRemaining);
    }

    public void EnemyKilled()
    {
        enemiesRemaining--;

        Debug.Log("Enemies remaining: " + enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        Debug.Log("LEVEL COMPLETE!");

        // Later:
        // Load next scene
        // Show UI
    }
}