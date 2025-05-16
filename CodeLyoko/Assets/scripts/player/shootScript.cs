using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootScript : MonoBehaviour
{
    public GameObject PrefabProjectile;
    public Transform SpawnPositionPrefab;
    public float ProjectileSpeed;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Camera principale non trouvée !");
        }
    }

    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainCamera == null)
            {
                Debug.LogError("Impossible de tirer : caméra principale introuvable !");
                return;
            }

            // Lancer un rayon depuis le centre de l'écran
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            // Calculer la direction du tir
            Vector3 shootDirection;
            if (Physics.Raycast(ray, out hit))
            {
                // Calculer la direction vers le point d'impact
                shootDirection = (hit.point - SpawnPositionPrefab.position).normalized;
            }
            else
            {
                shootDirection = ray.direction;
            }

            GameObject projectile = Instantiate(PrefabProjectile, SpawnPositionPrefab.position, Quaternion.identity);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootDirection * ProjectileSpeed;
            }
        }
    }
}
