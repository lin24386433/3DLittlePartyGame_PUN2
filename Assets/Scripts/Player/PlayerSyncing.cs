using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSyncing : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private PlayerModel playerModel = null;

    //Values that will be synced over network
    Vector3 latestPos;
    Quaternion latestRot;
    Vector3 latestVelocity;
    Vector3 latestAngularVelocity;
    //Lag compensation
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine)
        {
            //We own this player: send the others our data
            stream.SendNext(playerModel.transform.position);
            stream.SendNext(playerModel.transform.rotation);
            stream.SendNext(playerModel.Rigidbody.velocity);
            stream.SendNext(playerModel.Rigidbody.angularVelocity);
        }
        else
        {
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            latestVelocity = (Vector3)stream.ReceiveNext();
            latestAngularVelocity = (Vector3)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = playerModel.transform.position;
            rotationAtLastPacket = playerModel.transform.rotation;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            //Lag compensation
            float timeToReachGoal = (float)(currentPacketTime - lastPacketTime);
            currentTime += Time.deltaTime;

            Vector3 lagCompensationPos = new Vector3(timeToReachGoal * latestVelocity.x, timeToReachGoal * latestVelocity.y, timeToReachGoal * latestVelocity.z);
            Vector3 lagCompensationRot = new Vector3(timeToReachGoal * latestAngularVelocity.x, timeToReachGoal * latestAngularVelocity.y, timeToReachGoal * latestAngularVelocity.z);

            //Update remote player 
            playerModel.transform.position = Vector3.Lerp(positionAtLastPacket, latestPos + lagCompensationPos, (float)(currentTime / timeToReachGoal));
            playerModel.transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot * Quaternion.Euler(lagCompensationRot), (float)(currentTime / timeToReachGoal));
        }
    }
}
