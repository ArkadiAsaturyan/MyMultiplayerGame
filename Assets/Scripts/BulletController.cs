using DG.Tweening;
using ModestTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



public class BulletController : MonoBehaviour, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }


    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer sprite;

    private PhotonView view;
    private Vector2 bulletsPosition = new Vector2(10f, 10f);


    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet" || collision.tag == "Coin")
        {
            return;
        }

        if (view.IsMine)
        {

            /*transform.position = bulletsPosition;
            gameObject.SetActive(false);*/

            sprite.enabled = false;
            StartCoroutine(DestroyBullet());
        }

        else
        {
            sprite.enabled = false;
        }
    }


    public void Shoot(Vector2 direction)
    {

        if (direction != Vector2.zero)
        {
            rigidbody2D.AddForce(direction * speed);
        }
    }

}
