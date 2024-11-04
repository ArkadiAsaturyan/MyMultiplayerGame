using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class WinnerPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI winnerName;
        [SerializeField] private TextMeshProUGUI collectedCoins;
        [SerializeField] private Button backToMenuButton;

        private void Start()
        {
            backToMenuButton.onClick.AddListener(GoToMenu);
        }

        public void Setup(string name, int coins)
        {
            winnerName.text = $"{name} Wins!";
            collectedCoins.text = $"Collected Coins: <color=#1C8009>{coins}</color>";
        }

        private void GoToMenu()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Loading");
        }
    }
}
