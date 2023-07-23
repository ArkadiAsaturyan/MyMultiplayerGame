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
    [SerializeField] private ColorConfig colorConfig;
    [SerializeField] private int health;
    [SerializeField] private float bulletOfset;
    [SerializeField] private HealthBar healthBarPrefab;
    [SerializeField] private Vector3 healthBarOffset;

    public event Action<PlayerController> OnPlayerDestroyed;
    private string name;
    public string Name => name;

    private int collectedCoins;
    public int CollectedCoins => collectedCoins;

    private void AddPoint()
    {
        collectedCoins++;
    }

    private Color[] colors = { Color.white, Color.magenta, Color.red, Color.black, Color.yellow };
    private JoystickController2 _joystickController2;
    private Shoot _shoot;
    private PhotonView photonView;
    private Vector2 playerDirection = Vector2.zero;
    public Vector2 PlayerDirection => playerDirection;
    private HealthBar healthManager;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            GameObject healthBarGameObject = PhotonNetwork.Instantiate(healthBarPrefab.gameObject.name, transform.position + healthBarOffset, Quaternion.identity);
            healthManager = healthBarGameObject.GetComponent<HealthBar>();
            healthManager.Initialize(transform);
        }

    }

    private IEnumerator FindHealthBar()
    {
        while (healthManager == null)
        {
            var healthBarPhotonView = PhotonNetwork.PhotonViews
                .Where(v => v.CreatorActorNr == photonView.CreatorActorNr && v.TryGetComponent<HealthBar>(out HealthBar health)).FirstOrDefault();


            if (healthBarPhotonView != null)
            {
                healthManager = healthBarPhotonView.GetComponent<HealthBar>();
            }
            yield return healthManager;
        }

    }

    private IEnumerator FreezePLayerWhenAlone(JoystickController2 joystickController2, Shoot shoot)
    {
        while (PhotonNetwork.PhotonViews.Where(v => v.gameObject.tag == "Player").ToList().Count < 2)
        {
            yield return null;
        }
        playerDirection = Vector2.right;
        _joystickController2 = joystickController2;
        _shoot = shoot;
        _shoot.OnShoot += Shoot;
    }

    public void Initialize(JoystickController2 joystickController2, Shoot shoot)
    {

        StartCoroutine(FreezePLayerWhenAlone(joystickController2, shoot));
    }



    void Start()
    {
        spriteRenderer.color = colors[colorConfig.PlayerIndex];
        colorConfig.IncrementPlayerIndex();



        /*GameObject joystick = GameObject.FindWithTag("JoystickBG");
        joystickController2 = joystick.GetComponent<JoystickController2>();

        GameObject shootButton = GameObject.FindWithTag("ShootButton");
        shoot = shootButton.GetComponent<Shoot>();

        shoot.OnShoot += Shoot;*/

        if (healthManager == null)
        {
            StartCoroutine(FindHealthBar());
        }
        name = photonView.Owner.NickName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (collision.gameObject.GetComponent<BulletController>().IsInactive)
            {
                Debug.Log("bullet is inactive");
                return;
            }

            if (collision.gameObject.GetComponent<PhotonView>().Owner == photonView.Owner)
            {
                Debug.Log("bullet is yours");
                return;
            }
            if (photonView.IsMine)
            {
                healthManager.SetHealth();
            }

            health -= 10;
            Debug.Log("health: " + health);

            if (health <= 0)
            {
                Debug.Log("health <=0: " + health);

                if (photonView.IsMine)
                {
                    Debug.Log("IS mine. can destroy");
                    PhotonNetwork.Destroy(gameObject);
                    PhotonNetwork.Destroy(healthManager.gameObject);

                }
            }
        }

        else if (collision.gameObject.tag == "Coin")
        {

            AddPoint();
            Debug.Log($"{photonView.Owner.NickName}'s coins: {collectedCoins}");
        }
    }




    private void Shoot()
    {
        if (photonView.IsMine)
        {
            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);

            playerDirection = playerDirection.normalized;
            GameObject bulletGameObject = PhotonNetwork.Instantiate(bulletPrefab.name, playerPosition + playerDirection * bulletOfset, Quaternion.identity);
            //bullet.transform.position = playerPosition + playerDirection * 0.45f;

            //await Task.Delay(100);
            //bullet.SetActive(true);

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

    public void Move()
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

            transform.position += new Vector3(_joystickController2.InputDirection.x, _joystickController2.InputDirection.y, 0) * (m_Speed * Time.deltaTime);
            //playerDirection = _joystickController2.InputDirection;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Move();
        }

    }

    private void OnDestroy()
    {
        OnPlayerDestroyed.Invoke(this);
        colorConfig.DecrementPlayerIndex();
        StopCoroutine(FindHealthBar());
    }
}
