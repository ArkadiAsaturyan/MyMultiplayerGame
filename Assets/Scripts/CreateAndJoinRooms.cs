using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_InputField createInputField;
        [SerializeField] private TMP_InputField joinInputField;
        [SerializeField] private Button CreateButton;
        [SerializeField] private Button JoinButton;

        
       
        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom(createInputField.text);
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinInputField.text);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.NickName = nameInputField.text;
            PhotonNetwork.LoadLevel("Game");

        }
         
    }
}
