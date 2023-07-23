using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Assets
{
    public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_InputField createInputField;
        [SerializeField] private TMP_InputField joinInputField;
        [SerializeField] private Button createButton;
        [SerializeField] private Button joinButton;

        private void Start()
        {
            createButton.onClick.AddListener(CreateRoom);
            joinButton.onClick.AddListener(JoinRoom);
        }

        public void CreateRoom()
        {
            if(nameInputField.text == "" || createInputField.text == "")
            {
                return;
            }
            PhotonNetwork.CreateRoom(createInputField.text);
        }

        public void JoinRoom()
        {
            if (nameInputField.text == "" || joinInputField.text == "")
            {
                return;
            }
            PhotonNetwork.JoinRoom(joinInputField.text);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.NickName = nameInputField.text;
            PhotonNetwork.LoadLevel("Game");
        }         
    }
}
