using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float minX;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;
    [SerializeField] private CoinBar coinBar;
    [SerializeField] private int coinsAmount;

    private Vector2 randomPosition;
    List<PhotonView> coins = new List<PhotonView>();

    void Start()
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
        while (coins.Count != coinsAmount)
        {
            coins = PhotonNetwork.PhotonViews.Where(c => c.gameObject.tag == "Coin").ToList();
            yield return null;
        }

        foreach (var coin in coins)
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
            randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            GameObject coinGameObject = PhotonNetwork.Instantiate(coinPrefab.name, randomPosition, Quaternion.identity);
            CoinController coin = coinGameObject.GetComponent<CoinController>();
            coin.OnCoinCollected += coinBar.CoinCollected;
            coin.OnDestroyCoin += DestroyCoin;
        }
    }
}
