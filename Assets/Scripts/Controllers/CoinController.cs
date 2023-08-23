using System;
using Photon.Pun;
using UnityEngine;

namespace Controllers
{
    public class CoinController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public event Action<Collider2D> OnCoinCollected;
        public event Action<GameObject> OnDestroyCoin;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }
            gameObject.SetActive(false);
            OnCoinCollected?.Invoke(collision);
            if (PhotonNetwork.IsMasterClient)
            {
                OnDestroyCoin?.Invoke(gameObject);
            }
        }
    }
}
