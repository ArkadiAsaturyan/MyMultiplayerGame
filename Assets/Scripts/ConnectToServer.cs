using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {

        [Inject]
        public void Construct()
        {
            Debug.Log("Construct in connect to server");
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
