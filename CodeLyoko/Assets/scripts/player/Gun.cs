using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int maxAmmo = 10;
    private int currentAmmo;
    public float fireRate = 0.2f;
    public float reloadTime = 2f;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    public Transform SpawnPositionPrefab;
    public GameObject PrefabProjectile;
    public float ProjectileSpeed = 20f;
    public Camera mainCamera;

    private bool isEquipped = false;
    [SerializeField] private RectTransform viseur;
    public bool IsEquipped
    {
        get { return isEquipped; }
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (!isEquipped || isReloading) return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
        if (!isEquipped) return;

        if (currentAmmo <= 0)
        {
            Debug.Log("Plus de munitions, rechargez !");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogError("Impossible de tirer : caméra principale introuvable !");
            return;
        }

        nextFireTime = Time.time + fireRate;
        currentAmmo--;
        Vector3 aimPoint = viseur.position;

        Ray ray = mainCamera.ScreenPointToRay(aimPoint);
        RaycastHit hit;
        Vector3 shootDirection = ray.direction;

        if (Physics.Raycast(ray, out hit))
        {
            shootDirection = (hit.point - SpawnPositionPrefab.position).normalized;
        }

        SpawnPositionPrefab.forward = shootDirection;

        GameObject projectile = Instantiate(PrefabProjectile, SpawnPositionPrefab.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * ProjectileSpeed;
        }

        Debug.Log("Balles restantes: " + currentAmmo);
    }

    private IEnumerator Reload()
    {
        if (currentAmmo == maxAmmo) yield break;

        isReloading = true;
        Debug.Log("Rechargement...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Rechargé !");
    }

    public void PickUp()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            isEquipped = true;
            transform.SetParent(player.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            gameObject.SetActive(true);
            Debug.Log("Arme ramassée !");
        }
        else
        {
            Debug.LogError("Le joueur n'a pas été trouvé !");
        }
    }
}
