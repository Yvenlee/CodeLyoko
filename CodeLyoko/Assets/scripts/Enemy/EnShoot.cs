using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnShoot : MonoBehaviour
{
    public GameObject EnemyPrefabProjectile;
    public Transform EnemySpawnPositionPrefab;
    public float EnemyProjectileSpeed;
    public float shootInterval = 2f;
    public float projectileLifeTime = 5f;
    public int poolSize = 10;

    private bool isShooting = false;
    private Transform playerTransform;
    private Queue<GameObject> projectilePool = new Queue<GameObject>();
    private EnemyComportement enemyComportement;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyComportement = GetComponent<EnemyComportement>();
        InitializeProjectilePool();
    }

    void InitializeProjectilePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(EnemyPrefabProjectile);
            projectile.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
    }

    GameObject GetProjectileFromPool()
    {
        if (projectilePool.Count > 0)
        {
            GameObject projectile = projectilePool.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }
        else
        {
            return Instantiate(EnemyPrefabProjectile);
        }
    }

    public void ReturnProjectileToPool(GameObject projectile)
    {
        projectile.SetActive(false);
        projectilePool.Enqueue(projectile);
    }

    public void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            StartCoroutine(ShootAtPlayer());
        }
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    IEnumerator ShootAtPlayer()
    {
        while (isShooting)
        {
            GameObject projectile = GetProjectileFromPool();
            projectile.transform.position = EnemySpawnPositionPrefab.position;
            Vector3 directionToPlayer = (playerTransform.position - EnemySpawnPositionPrefab.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(directionToPlayer);
            projectile.GetComponent<Rigidbody>().AddForce(directionToPlayer * EnemyProjectileSpeed, ForceMode.VelocityChange);

            if (GetComponent<AudioSource>())
                GetComponent<AudioSource>().Play();
            EnemyLaserScript laserScript = projectile.GetComponent<EnemyLaserScript>();
            if (laserScript != null && enemyComportement != null)
            {
                laserScript.Initialize(this, enemyComportement);
            }

            StartCoroutine(HandleProjectileLifeCycle(projectile));

            yield return new WaitForSeconds(shootInterval);
        }
    }

    IEnumerator HandleProjectileLifeCycle(GameObject projectile)
    {
        yield return new WaitForSeconds(projectileLifeTime);
        ReturnProjectileToPool(projectile);
    }
}
