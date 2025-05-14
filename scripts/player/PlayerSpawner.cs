using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject player;
    public Transform spawnPoint;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && spawnPoint != null)
        {
            Instantiate(player, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Player prefab or spawn point is not assigned.");
        }
    }
}
