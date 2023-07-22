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

    private Vector2 randomPosition;

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
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
        yield return new WaitForSeconds(1);

        var coins = PhotonNetwork.PhotonViews.Where(c => c.gameObject.tag == "Coin").ToList();
        foreach (var coin in coins)
        {
            coin.gameObject.GetComponent<Coin>().OnCoinCollected += coinBar.CoinCollected;
        }
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < 10; i++)
        {
            randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            GameObject coinGameObject = PhotonNetwork.Instantiate(coinPrefab.name, randomPosition, Quaternion.identity);
            Coin coin = coinGameObject.GetComponent<Coin>();
            coin.OnCoinCollected += coinBar.CoinCollected;
        }
    }

}
