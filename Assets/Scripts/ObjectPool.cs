using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletsCount;

    private PhotonView photonview;
    private List<GameObject> bullets;
    private Vector2 bulletsPosition = new Vector2(10f, 10f);


    void Start()
    {
        photonview = GetComponent<PhotonView>();

        if (photonview.IsMine)
        {
            GameObject bullet;
            bullets = new List<GameObject>();

            for (int i = 0; i < bulletsCount; i++)
            {
                bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletsPosition, Quaternion.identity);
                bullet.SetActive(false);
                bullets.Add(bullet);
            }
        }
    }

    public GameObject GetBulletFromObjectPool()
    {
        for (int i = 0; i < bulletsCount; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return bullets[i];
            }
        }
        return null;
    }

}
