using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action<Collider2D> OnCoinCollected;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            return;
        }

        if(collision.gameObject.tag == "Player")
        {
            OnCoinCollected(collision);
            spriteRenderer.enabled = false;
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Destroy coin");
                PhotonNetwork.Destroy(gameObject);
            }
            
        }
    }
}
