using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class CoinBar : MonoBehaviour
    {
        [SerializeField] private Image progress;
        [SerializeField] private TextMeshProUGUI collectedCoinsText;
        [SerializeField] private float coinsAmount;

        private float _oneCoinProgress;

        private void Start()
        {
            _oneCoinProgress = 1f / coinsAmount;
        }

        private int _collectedCoins;
        public void CoinCollected(Collider2D collision)
        {
            if(collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                _collectedCoins++;
                collectedCoinsText.text = $"{_collectedCoins}";
                progress.fillAmount += _oneCoinProgress;
            }
        }
    }
}
