using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }
        else
        {
            Debug.LogError("Player not found.");
        }
    }
}
