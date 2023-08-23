using Controllers;
using Photon.Pun;
using UnityEngine;

namespace Spawners
{
    public class SpawnPlayers : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private float minX;
        [SerializeField] private float minY;
        [SerializeField] private float maxX;
        [SerializeField] private float maxY;
        [SerializeField] private JoystickController joystickController;
        [SerializeField] private ShootController shootController;

        private PlayerController _playerController;

        private void Start()
        {
            Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
            _playerController = player.GetComponent<PlayerController>();
            _playerController.Initialize(joystickController, shootController);
        }    
    }
}
