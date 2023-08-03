using System;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace LagCompensation
{
    public class LagCompensation : MonoBehaviourPun, IPunObservable
    {
        private Vector2 latestPos;
        private Quaternion latestRot;
        private float currentTime = 0;
        private double currentPacketTime = 0;
        private double lastPacketTime = 0;
        private Vector2 positionAtLastPacket = Vector3.zero;
        private Quaternion rotationAtLastPacket = quaternion.identity;

        public float smoothPos = 0.5f;

        private void Awake()
        {
            PhotonNetwork.SendRate = 30;
            PhotonNetwork.SerializationRate = 10;
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
                latestPos = (Vector3)stream.ReceiveNext();
                latestRot = (Quaternion)stream.ReceiveNext();

                currentTime = 0.0f;
                lastPacketTime = currentPacketTime;
                currentPacketTime = info.SentServerTime;
                positionAtLastPacket = transform.position;
                rotationAtLastPacket = transform.rotation;
            }
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                return;
            }

            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));

        }
    }
}