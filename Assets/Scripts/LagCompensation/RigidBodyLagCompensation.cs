using UnityEngine;
using Photon.Pun;

public class RigidBodyLagCompensation : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Rigidbody2D rigidbody;

    private void Awake()
    {
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rigidbody.rotation);
        }
        else
        {
            rigidbody.rotation = (float)stream.ReceiveNext();
        }
    }
}
