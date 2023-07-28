using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action<Collider2D> OnCoinCollected;
    public event Action<GameObject> OnDestroyCoin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            return;
        }

        if(collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            OnCoinCollected(collision);
            if (PhotonNetwork.IsMasterClient)
            {
                OnDestroyCoin(gameObject);
            }
        }
    }
}
