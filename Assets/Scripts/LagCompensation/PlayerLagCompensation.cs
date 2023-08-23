using System;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace LagCompensation
{
    public class PlayerLagCompensation : MonoBehaviourPun, IPunObservable
    {
        private Vector2 _latestPos;
        private Quaternion _latestRot;
        private float _currentTime = 0;
        private double _currentPacketTime = 0;
        private double _lastPacketTime = 0;
        private Vector2 _positionAtLastPacket = Vector3.zero;
        private Quaternion _rotationAtLastPacket = quaternion.identity;
        private Transform _transform1;
        public float smoothPos = 0.5f;

        private void Awake()
        {
            PhotonNetwork.SendRate = 30;
            PhotonNetwork.SerializationRate = 30;
            _transform1 = transform;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                _latestPos = (Vector3)stream.ReceiveNext();
                _latestRot = (Quaternion)stream.ReceiveNext();

                _currentTime = 0.0f;
                _lastPacketTime = _currentPacketTime;
                _currentPacketTime = info.SentServerTime;
                _positionAtLastPacket = _transform1.position;
                _rotationAtLastPacket = _transform1.rotation;
            }
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                return;
            }

            double timeToReachGoal = _currentPacketTime - _lastPacketTime;
            _currentTime += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(_positionAtLastPacket, _latestPos, (float)(_currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(_rotationAtLastPacket, _latestRot, (float)(_currentTime / timeToReachGoal));

        }
    }
}