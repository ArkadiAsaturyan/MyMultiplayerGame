using System;
using System.Collections;
using System.Linq;
using Config;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float mSpeed;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private PlayerConfig colorConfig;
        [SerializeField] private int health;
        [SerializeField] private float bulletOffset;
        [SerializeField] private HealthBar healthBarPrefab;
        [SerializeField] private Vector3 healthBarOffset;
        [SerializeField] private PhotonView photonView;

        private readonly Color[] _colors = { Color.green, Color.red, Color.magenta, Color.yellow, Color.black, Color.white };
        private JoystickController _joystickController2;
        private ShootController _shootController;
        private Vector2 _playerDirection = Vector2.zero;
        public Vector2 PlayerDirection => _playerDirection;
        private HealthBar _healthBar;

        public event Action<PlayerController> OnPlayerDestroyed;
        public string Name { get; private set; }

        public int CollectedCoins { get; private set; }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                GameObject healthBarGameObject = PhotonNetwork.Instantiate(healthBarPrefab.gameObject.name,
                    transform.position + healthBarOffset, Quaternion.identity);
                _healthBar = healthBarGameObject.GetComponent<HealthBar>();
                _healthBar.Initialize(transform);
            }
        }

        private void Start()
        {
            spriteRenderer.color = _colors[colorConfig.PlayerIndex];
            colorConfig.IncrementPlayerIndex();

            if (_healthBar == null)
            {
                StartCoroutine(FindHealthBar());
            }
            Name = photonView.Owner.NickName;
        }

        private void AddPoint()
        {
            CollectedCoins++;
        }

        private IEnumerator FindHealthBar()
        {
            while (_healthBar == null)
            {
                var healthBarPhotonView = PhotonNetwork.PhotonViews.FirstOrDefault(v => v.CreatorActorNr == photonView.CreatorActorNr 
                    && v.gameObject.CompareTag("Health"));

                if (healthBarPhotonView != null)
                {
                    _healthBar = healthBarPhotonView.GetComponent<HealthBar>();
                }
                yield return _healthBar;
            }
        }

        private IEnumerator FreezePLayerWhenAlone(JoystickController joystickController2, ShootController shootController)
        {
            while (PhotonNetwork.PhotonViews.Where(v => v.gameObject.CompareTag("Player")).ToList().Count < 2)
            {
                yield return null;
            }
            _playerDirection = Vector2.right;
            _joystickController2 = joystickController2;
            _shootController = shootController;
            _shootController.OnShoot += Shoot;
        }

        public void Initialize(JoystickController joystickController2, ShootController shootController)
        {
            StartCoroutine(FreezePLayerWhenAlone(joystickController2, shootController));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                if (collision.gameObject.GetComponent<BulletController>().IsInactive)
                {
                    return;
                }
                if (collision.gameObject.GetComponent<PhotonView>().Owner == photonView.Owner)
                {
                    return;
                }
                if (photonView.IsMine)
                {
                    _healthBar.SetHealth();
                }

                health -= 10;

                if (health <= 0 && photonView.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                    PhotonNetwork.Destroy(_healthBar.gameObject);
                }
            }

            else if (collision.gameObject.CompareTag("Coin"))
            {
                AddPoint();
            }
        }

        private void Shoot()
        {
            if (photonView.IsMine)
            {
                var position = transform.position;
                Vector2 playerPosition = new Vector2(position.x, position.y);
                _playerDirection = _playerDirection.normalized;
                GameObject bulletGameObject = PhotonNetwork
                    .Instantiate(bulletPrefab.name, playerPosition + _playerDirection * bulletOffset, Quaternion.identity);
                BulletController bulletController = bulletGameObject.GetComponent<BulletController>();
                bulletController.Shoot(_playerDirection);
                StartCoroutine(HideBullet(bulletGameObject));
            }
        }

        private IEnumerator HideBullet(GameObject bullet)
        {
            yield return new WaitForSeconds(3);
            if (bullet != null)
            {
                PhotonNetwork.Destroy(bullet);
            }
        }

        private void RotatePlayer()
        {
            _playerDirection = _joystickController2.InputDirection;
            float angle = Vector2.Angle(Vector2.right, _joystickController2.InputDirection);
            var transform1 = transform;
            var eulerAngles = transform1.eulerAngles;
            
            if (_joystickController2.InputDirection.y < 0)
            {
                eulerAngles = new Vector3(
                    eulerAngles.x,
                    eulerAngles.y,
                    -angle
                );
                transform1.eulerAngles = eulerAngles;
                return;
            }
            
            eulerAngles = new Vector3(
                eulerAngles.x,
                eulerAngles.y,
                angle
            );
            transform1.eulerAngles = eulerAngles;
        }

        private void Move()
        {
            if (_joystickController2 == null || _joystickController2.InputDirection == Vector2.zero)
            {
                return;
            }
            RotatePlayer();
            if ((transform.position.y < -4.1f && _joystickController2.InputDirection.y < 0)
                || (transform.position.y > 4.1f && _joystickController2.InputDirection.y > 0)
                || (transform.position.x < -7.6f && _joystickController2.InputDirection.x < 0)
                || (transform.position.x > 7.6f && _joystickController2.InputDirection.x > 0))
            {
                return;
            }
            transform.position += new Vector3
                (_joystickController2.InputDirection.x, _joystickController2.InputDirection.y, 0) * (mSpeed * Time.deltaTime);
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                Move();
            }
        }

        private void OnDestroy()
        {
            OnPlayerDestroyed?.Invoke(this);
            colorConfig.DecrementPlayerIndex();
            StopCoroutine(FindHealthBar());
        }
    }
}
