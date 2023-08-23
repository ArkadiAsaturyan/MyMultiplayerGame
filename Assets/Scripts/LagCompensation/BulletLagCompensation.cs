using Photon.Pun;
using UnityEngine;

namespace LagCompensation
{
    public class BulletLagCompensation : MonoBehaviour, IPunObservable
    {
        [SerializeField] private Rigidbody2D rigidbody;

        private PhotonView _photonView;
        private Vector2 _networkPosition;

        private void Awake()
        {
            PhotonNetwork.SendRate = 30;
            PhotonNetwork.SerializationRate = 30;
            _photonView = GetComponent<PhotonView>();
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
                _networkPosition = (Vector2)stream.ReceiveNext();
                rigidbody.velocity = (Vector2)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPosition += rigidbody.velocity * lag;
            }
        }

        public void FixedUpdate()
        {
            if (!_photonView.IsMine)
            {
                rigidbody.position = Vector3.MoveTowards(rigidbody.position, _networkPosition, Time.fixedDeltaTime);
            }
        }
    }
}
