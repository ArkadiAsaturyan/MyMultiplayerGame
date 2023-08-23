using TMPro;
using UnityEngine;

namespace UI
{
    public class WinnerPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI winnerName;
        [SerializeField] private TextMeshProUGUI collectedCoins;

        public void Setup(string name, int coins)
        {
            winnerName.text = $"{name} Wins!";
            collectedCoins.text = $"Collected Coins: {coins}";
        }
    }
}
