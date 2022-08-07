using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

/// <summary>
/// This class will take the data received by the OptitrackStreamingClient, serialize the data,
/// and send it to other Photon Views via an RPC.
/// 
/// </summary>
public class OptiTrackStateSender : MonoBehaviourPun
{
    public OptitrackStreamingClient client;

    public float time = 1f;
    public float frequency = 0.0666666667f;

    private void Start()
    {
        if (client == null) return;

        InvokeRepeating("SendOptitrackRigidbodyOrigins", time, frequency);
    }

    void SendOptitrackRigidbodyOrigins()
    {
        // Send each of the rigidbody states
        foreach (Int32 key in client.RigidBodyStates.Keys)
        {
            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Create a needle to test the rotation 
            Vector3 rbPos = client.RigidBodyStates[key].Pose.Position;
            Quaternion rbRot = client.RigidBodyStates[key].Pose.Orientation;

            SendOptitrackRBOrigin(rbPos, rbRot, key);
        }
    }


    private void SendOptitrackRBOrigin(Vector3 position, Quaternion rotation, int id)
    {

        print("Sending");
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        // We need to convert the list of optitrack data states to a serialized array

        object[] streamedData = new object[3];

        // store the OptitrackDataState count as the 0th element
        streamedData[0] = position;
        streamedData[1] = rotation;
        streamedData[2] = id;

        // Send optitrack data to all connected clients
        foreach (PhotonView p in photonViews)
        {
            if (p.TryGetComponent(out OptiTrackStateReceiver receiver))
            {
                //print("Found the streamed hand" + p.ViewID + p.transform.name);
                p.RPC("ReceiveOptitrackRBState", RpcTarget.AllViaServer, streamedData);
                break;
            }
        }

    }
}