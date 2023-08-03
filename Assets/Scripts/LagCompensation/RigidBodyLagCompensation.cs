using UnityEngine;
using Photon.Pun;

public class RigidBodyLagCompensation : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Rigidbody2D rigidbody;

    private Vector2 networkPosition;
    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rigidbody.rotation);
            stream.SendNext(rigidbody.velocity);
            stream.SendNext(rigidbody.position);
        }
        else
        {
            rigidbody.rotation = (float)stream.ReceiveNext();
            rigidbody.velocity = (Vector2)stream.ReceiveNext();
            rigidbody.position = (Vector2)stream.ReceiveNext();
            
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rigidbody.position += rigidbody.velocity * lag;
        }
        
    }
    
    // public void FixedUpdate()
    // {
    //     if (!photonView.IsMine)
    //     {
    //         //rigidbody.position = Vector2.MoveTowards(rigidbody.position, networkPosition, Time.fixedDeltaTime);
    //         rigidbody.position = Vector2.Lerp(rigidbody.position, networkPosition, Time.fixedDeltaTime);
    //     }
    // }
}
