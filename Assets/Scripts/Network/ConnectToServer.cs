using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            Debug.Log("ConnectToServer Start");
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
            SceneManager.LoadScene("Lobby");
        }
    }
}
