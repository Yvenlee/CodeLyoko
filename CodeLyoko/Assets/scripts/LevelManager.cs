using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int nextSceneIndex;

    private int totalSpawners;
    private int spawnersCleared;

    void Start()
    {
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        totalSpawners = spawners.Length;

        foreach (var spawner in spawners)
        {
            spawner.OnSpawnerCleared += SpawnerCleared;
        }
    }

    private void SpawnerCleared()
    {
        spawnersCleared++;

        if (spawnersCleared >= totalSpawners)
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
}
