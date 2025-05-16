using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyComportement : MonoBehaviour
{
    private EnShoot enShoot;
    private Vector3 randomDirection;
    public float moveSpeed = 2f;
    public float turnSpeed = 5f;
    public float changeDirectionTime = 3f;
    public float HP = 100f;
    private float maxHP;
    public float Damages = 0f;
    private bool isChasingPlayer = false;
    private Coroutine moveCoroutine;
    private EnemyMovements enemyMovements;
    public Slider healthBar;

    public delegate void EnemyDestroyedEvent();
    public event EnemyDestroyedEvent OnEnemyDestroyed;

    void Start()
    {
        enShoot = GetComponent<EnShoot>();
        enemyMovements = GetComponent<EnemyMovements>();
        moveCoroutine = StartCoroutine(MoveRandomly());

        maxHP = HP;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHP;
            healthBar.value = HP;
        }
    }

    void Update()
    {
        if (!isChasingPlayer)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damageTaken)
    {
        HP -= damageTaken;

        if (healthBar != null)
        {
            healthBar.value = HP;
        }

        if (HP <= 0)
        {
            if (OnEnemyDestroyed != null)
            {
                OnEnemyDestroyed();
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasingPlayer = true;
            enShoot.StartShooting();
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
            enemyMovements.StartChasingPlayer();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasingPlayer = false;
            enShoot.StopShooting();
            enemyMovements.StopChasingPlayer();
            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(MoveRandomly());
            }
        }
    }

    void AvoidObstacles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            randomDirection = Vector3.Reflect(transform.forward, hit.normal);
        }
    }

    IEnumerator MoveRandomly()
    {
        while (!isChasingPlayer)
        {
            AvoidObstacles();
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            Quaternion newRotation = Quaternion.LookRotation(randomDirection);
            float elapsedTime = 0f;

            while (elapsedTime < changeDirectionTime / 2)
            {
                if (isChasingPlayer) yield break;
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(changeDirectionTime);
        }
    }
}
