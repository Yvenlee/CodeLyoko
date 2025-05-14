using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserScript : MonoBehaviour
{
    private float damage;
    private EnShoot enShoot;
    private EnemyComportement enemyComportement;

    public void Initialize(EnShoot shootScript, EnemyComportement enemy)
    {
        enShoot = shootScript;
        enemyComportement = enemy;
        damage = enemyComportement.Damages;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Appel de la fonction pour infliger des dégâts au joueur
            PlayerController1 player = other.gameObject.GetComponent<PlayerController1>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // Retour au pool pour les projectiles avec collision physique
            enShoot.ReturnProjectileToPool(gameObject);
        }
        else
        {
            // Si le projectile touche autre chose que le joueur, on le retourne dans le pool
            enShoot.ReturnProjectileToPool(gameObject);
        }
    }

    // Méthode pour détecter les entrées dans le trigger (pour les projectiles en mode Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Appel de la fonction pour infliger des dégâts au joueur
            PlayerController1 player = other.GetComponent<PlayerController1>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
