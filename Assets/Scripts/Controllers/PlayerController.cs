using System.Collections;
using UnityEngine;
using Photon.Pun;
using Assets;
using System;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerConfig colorConfig;
    [SerializeField] private int health;
    [SerializeField] private float bulletOfset;
    [SerializeField] private HealthBar healthBarPrefab;
    [SerializeField] private Vector3 healthBarOffset;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Rigidbody2D rigidbody2D;


    private Color[] colors = { Color.green, Color.red, Color.magenta, Color.yellow, Color.black, Color.white };
    private JoystickController _joystickController2;
    private ShootController _shootController;
    private Vector2 playerDirection = Vector2.zero;
    public Vector2 PlayerDirection => playerDirection;
    private HealthBar healthBar;

    public event Action<PlayerController> OnPlayerDestroyed;
    private string name;
    public string Name => name;

    private int collectedCoins;
    public int CollectedCoins => collectedCoins;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            GameObject healthBarGameObject = PhotonNetwork.Instantiate(healthBarPrefab.gameObject.name,
                transform.position + healthBarOffset, Quaternion.identity);
            healthBar = healthBarGameObject.GetComponent<HealthBar>();
            healthBar.Initialize(transform);
        }
    }

    void Start()
    {
        
        spriteRenderer.color = colors[colorConfig.PlayerIndex];
        colorConfig.IncrementPlayerIndex();

        if (healthBar == null)
        {
            StartCoroutine(FindHealthBar());
        }
        name = photonView.Owner.NickName;
    }

    private void MovePlayer()
    {
        if (photonView.IsMine)
        {
            MoveForDrag();
        }
    }
    
    private void AddPoint()
    {
        collectedCoins++;
    }

    private IEnumerator FindHealthBar()
    {
        while (healthBar == null)
        {
            var healthBarPhotonView = PhotonNetwork.PhotonViews
                .Where(v => v.CreatorActorNr == photonView.CreatorActorNr 
                && v.gameObject.tag == "Health").FirstOrDefault();

            if (healthBarPhotonView != null)
            {
                healthBar = healthBarPhotonView.GetComponent<HealthBar>();
            }
            yield return healthBar;
        }
    }

    private IEnumerator FreezePLayerWhenAlone(JoystickController joystickController2, ShootController shootController)
    {
        while (PhotonNetwork.PhotonViews.Where(v => v.gameObject.tag == "Player").ToList().Count < 2)
        {
            yield return null;
        }
        playerDirection = Vector2.right;
        _joystickController2 = joystickController2;
        _shootController = shootController;
        _shootController.OnShoot += Shoot;
        _joystickController2.OnDragStart += MovePlayer;
        _joystickController2.OnDragDetected += MovePlayer;
        _joystickController2.OnDragEnd += StopMove;
    }

    public void Initialize(JoystickController joystickController2, ShootController shootController)
    {
        StartCoroutine(FreezePLayerWhenAlone(joystickController2, shootController));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
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
                healthBar.SetHealth();
            }

            health -= 10;

            if (health <= 0 && photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
                PhotonNetwork.Destroy(healthBar.gameObject);
            }
        }

        else if (collision.gameObject.tag == "Coin")
        {
            AddPoint();
        }
    }

    private void Shoot()
    {
        if (photonView.IsMine)
        {
            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
            playerDirection = playerDirection.normalized;
            GameObject bulletGameObject = PhotonNetwork
                .Instantiate(bulletPrefab.name, playerPosition + playerDirection * bulletOfset, Quaternion.identity);
            BulletController bulletController = bulletGameObject.GetComponent<BulletController>();
            bulletController.Shoot(playerDirection);
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
        playerDirection = _joystickController2.InputDirection;
        float angle = Vector2.Angle(Vector2.right, _joystickController2.InputDirection);
        transform.rotation = new Quaternion(0, 0, angle, 0);

        if (_joystickController2.InputDirection.y < 0)
        {
            transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    -angle
                );
            return;
        }

        transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    angle
                );
    }

    private void StopMove()
    {
        rigidbody2D.velocity = Vector2.zero;
    }
    
    private void Move()
    {
        if (_joystickController2 != null && _joystickController2.InputDirection != Vector2.zero)
        {
            RotatePlayer();
            if ((transform.position.y < -4.1f && _joystickController2.InputDirection.y < 0)
                || (transform.position.y > 4.1f && _joystickController2.InputDirection.y > 0)
                || (transform.position.x < -7.6f && _joystickController2.InputDirection.x < 0)
                || (transform.position.x > 7.6f && _joystickController2.InputDirection.x > 0))
            {
                return;
            }
            transform.position += new Vector3
                (_joystickController2.InputDirection.x, _joystickController2.InputDirection.y, 0) * (m_Speed * Time.deltaTime);
        }
    }

    private void MoveForDrag()
    {
        if (_joystickController2 != null && _joystickController2.InputDirection != Vector2.zero)
        {
            RotatePlayer();
            if ((transform.position.y < -4.1f && _joystickController2.InputDirection.y < 0)
                || (transform.position.y > 4.1f && _joystickController2.InputDirection.y > 0)
                || (transform.position.x < -7.6f && _joystickController2.InputDirection.x < 0)
                || (transform.position.x > 7.6f && _joystickController2.InputDirection.x > 0))
            {
                return;
            }
            
            rigidbody2D.velocity = _joystickController2.InputDirection * m_Speed;
        }
    }
    
    // void Update()
    // {
    //     Debug.LogError("Update");
    //      if (photonView.IsMine)
    //      {
    //          Move();
    //      }
    // }

    private void OnDestroy()
    {
        OnPlayerDestroyed.Invoke(this);
        colorConfig.DecrementPlayerIndex();
        StopCoroutine(FindHealthBar());
    }
}
