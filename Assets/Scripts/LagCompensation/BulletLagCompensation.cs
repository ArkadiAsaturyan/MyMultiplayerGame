using Photon.Pun;
using UnityEngine;

public class BulletLagCompensation : MonoBehaviour, IPunObservable
{
    [SerializeField] Rigidbody2D rigidbody;

    private PhotonView photonView;
    private Vector2 networkPosition;

    private void Awake()
    {
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
        photonView = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.velocity);
        }
        else
        {
            networkPosition = (Vector2)stream.ReceiveNext();
            rigidbody.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            networkPosition += rigidbody.velocity * lag;
        }
    }

    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rigidbody.position = Vector3.MoveTowards(rigidbody.position, networkPosition, Time.fixedDeltaTime);
        }
    }
}
