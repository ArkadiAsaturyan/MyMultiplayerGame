using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action<Collider2D> OnCoinCollected;

    private IEnumerator DestroyCoinWithDelay()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.Destroy(gameObject);
        Debug.Log("Destroy coin");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            return;
        }

        if(collision.gameObject.tag == "Player")
        {
            OnCoinCollected(collision);
            //spriteRenderer.enabled = false;
            gameObject.SetActive(false);
            /*if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(DestroyCoinWithDelay());
            }*/
            
        }
    }
}
