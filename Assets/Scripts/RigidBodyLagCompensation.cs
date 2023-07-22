using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

public class RigidBodyLagCompensation : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rigidbody;
    private Vector2 bulletsPosition = new Vector2(10f, 10f);

    private void Awake()
    {
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.rotation);
            stream.SendNext(rigidbody.velocity);
        }
        else
        {
            rigidbody.position = (Vector2)stream.ReceiveNext();
            rigidbody.rotation = (float)stream.ReceiveNext();
            rigidbody.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rigidbody.position += rigidbody.velocity * lag;
        }

    }

    private void FixedUpdate()
    {
        /*if(Vector2.Distance(rigidbody.position, bulletsPosition) > 3)
        {
            rigidbody.position
        }*/
    }
}
