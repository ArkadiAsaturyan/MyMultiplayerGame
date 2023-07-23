using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer sprite;

    private PhotonView photonView;
    private bool isInactive;
    public bool IsInactive => isInactive;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet" || collision.tag == "Coin" 
            || (collision.gameObject.GetComponent<PhotonView>().Owner == photonView.Owner))
        {
            return;
        }

        if (photonView.IsMine)
        {
            StartCoroutine(DestroyBullet());
        }
        sprite.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Bullet" || collision.tag == "Coin"
            || (collision.gameObject.GetComponent<PhotonView>().Owner == photonView.Owner))
        {
            return;
        }
        isInactive = true;
    }

    public void Shoot(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            rigidbody2D.AddForce(direction * speed);
        }
    }
}
