using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Spawners
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private float minX;
        [SerializeField] private float minY;
        [SerializeField] private float maxX;
        [SerializeField] private float maxY;
        [SerializeField] private CoinBar coinBar;
        [SerializeField] private int coinsAmount;

        private Vector2 _randomPosition;
        List<PhotonView> _coins = new List<PhotonView>();

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnCoins();
            }
            else
            {
                StartCoroutine(SubscribeEventForNonMasterClientCoroutine());
            }
        }

        private IEnumerator SubscribeEventForNonMasterClientCoroutine()
        {
            while (_coins.Count != coinsAmount)
            {
                _coins = PhotonNetwork.PhotonViews.Where(c => c.gameObject.CompareTag("Coin")).ToList();
                yield return null;
            }

            foreach (var coin in _coins)
            {
                coin.gameObject.GetComponent<CoinController>().OnCoinCollected += coinBar.CoinCollected;
            }
        }

        private void DestroyCoin(GameObject coin)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(DestroyCoinWithDelay(coin));
            }
        }

        private IEnumerator DestroyCoinWithDelay(GameObject coin)
        {
            yield return new WaitForSeconds(1);
            PhotonNetwork.Destroy(coin);
        }

        private void SpawnCoins()
        {
            for (int i = 0; i < coinsAmount; i++)
            {
                _randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                GameObject coinGameObject = PhotonNetwork.Instantiate(coinPrefab.name, _randomPosition, Quaternion.identity);
                CoinController coin = coinGameObject.GetComponent<CoinController>();
                coin.OnCoinCollected += coinBar.CoinCollected;
                coin.OnDestroyCoin += DestroyCoin;
            }
        }
    }
}
