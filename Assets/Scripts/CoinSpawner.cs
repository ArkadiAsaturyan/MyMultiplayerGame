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
            Debug.Log("PhotonNetwork.IsMasterClient else");
            StartCoroutine(SubscribeEventForNonMasterClientCoroutine());
        }
    }


    private IEnumerator SubscribeEventForNonMasterClientCoroutine()
    {
        while (coins.Count != coinsAmount)
        {
            Debug.Log("in while");

            coins = PhotonNetwork.PhotonViews.Where(c => c.gameObject.tag == "Coin").ToList();
            Debug.Log("coins count: " + coins.Count);

            yield return null;
        }
        Debug.Log("after while");

        foreach (var coin in coins)
        {
            coin.gameObject.GetComponent<Coin>().OnCoinCollected += coinBar.CoinCollected;
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
        Debug.Log("Destroy coin");
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < coinsAmount; i++)
        {
            randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            GameObject coinGameObject = PhotonNetwork.Instantiate(coinPrefab.name, randomPosition, Quaternion.identity);
            Coin coin = coinGameObject.GetComponent<Coin>();
            coin.OnCoinCollected += coinBar.CoinCollected;
            coin.OnDestroyCoin += DestroyCoin;
        }
    }

}
