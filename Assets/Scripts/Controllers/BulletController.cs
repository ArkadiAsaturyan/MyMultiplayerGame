using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Controllers
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private float speed;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private PhotonView photonView;

        public bool IsInactive { get; private set; }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(0.5f);
            PhotonNetwork.Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet") || collision.CompareTag("Coin") 
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
            if (collision.CompareTag("Bullet") || collision.CompareTag("Coin")
                                               || (collision.gameObject.GetComponent<PhotonView>().Owner == photonView.Owner))
            {
                return;
            }
            IsInactive = true;
        }

        public void Shoot(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                rigidbody2D.AddForce(direction * speed);
            }
        }
    }
}
