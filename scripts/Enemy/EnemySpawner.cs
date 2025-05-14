using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 5;
    public float spawnInterval = 1.0f;

    private SphereCollider spawnAreaCollider;
    private int activeEnemies = 0;
    private ParticleSystem[] particleSystems;
    private AudioClip tourdesactivee;

    public delegate void SpawnerClearedEvent();
    public event SpawnerClearedEvent OnSpawnerCleared;

    void Start()
    {
        spawnAreaCollider = GetComponent<SphereCollider>();
        if (spawnAreaCollider == null)
        {
            Debug.LogError("No SphereCollider found on the GameObject.");
            return;
        }
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(SpawnEnemies());
        tourdesactivee = GetComponent<AudioClip>();
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            activeEnemies++;

            EnemyComportement enemyScript = enemy.GetComponent<EnemyComportement>();
            if (enemyScript != null)
            {
                enemyScript.OnEnemyDestroyed += EnemyDestroyed;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomPosition()
    {
        float radius = spawnAreaCollider.radius;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0;
        return transform.position + randomDirection;
    }

    private void EnemyDestroyed()
    {
        activeEnemies--;
        if (activeEnemies <= 0)
        {
            if (GetComponent<AudioSource>())
                GetComponent<AudioSource>().Play();
            ChangeParticleColorToBlue();
            if (OnSpawnerCleared != null)
            {
                OnSpawnerCleared();
            }
        }
    }
    private void ChangeParticleColorToBlue()
    {
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.startColor = Color.blue;
        }
    }
}
