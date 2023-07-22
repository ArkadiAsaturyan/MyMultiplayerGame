using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControlller : MonoBehaviour
{

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float minX;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;

    void Start()
    {
        Vector2 randomPosition;
        randomPosition= new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject coin = PhotonNetwork.Instantiate(coinPrefab.name, randomPosition, Quaternion.identity);
        randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject coin2 = PhotonNetwork.Instantiate(coinPrefab.name, randomPosition, Quaternion.identity);
        randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject con3 = PhotonNetwork.Instantiate(coinPrefab.name, randomPosition, Quaternion.identity);
        randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject coin4 = PhotonNetwork.Instantiate(coinPrefab.name, randomPosition, Quaternion.identity);
    }

}
