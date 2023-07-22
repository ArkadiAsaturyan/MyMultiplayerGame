using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float minX;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;
    [SerializeField] private JoystickController2 joystickController2;
    [SerializeField] private Shoot shoot;

    private PlayerController playerController;
    void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        playerController = player.GetComponent<PlayerController>();
        playerController.Initialize(joystickController2, shoot);
    }

    
}
