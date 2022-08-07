using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will contain a PUN RPC Method to receive the serialized optitrack data
/// This call will then deserialize the data into Transform data, and use the Transform data
/// for visualization(e.g white spheres or needle)
/// </summary>
public class OptiTrackStateReceiver : MonoBehaviour
{
    const int NEEDLE_ID = 0;
    const int PHANTOM_ID = 1;

    [SerializeField]
    private GameObject _needlePrefab;

    [SerializeField]
    private GameObject _ctrScanPrefab;

    [SerializeField]
    private GameObject _vufImageTarget;

    [SerializeField]
    private Transform _optiTrackItemsParent;

    public struct OptitrackRBOriginData
    {
        public Vector3 position;
        public Quaternion rotation;

        public OptitrackRBOriginData(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    Dictionary<int, OptitrackRBOriginData> optiTrackDataDictionary;

    Dictionary<int, TrackedItem> gameObjectDictionary;

    private ImageTargetHandler _imageTargetHandler;



    // Start is called before the first frame update
    void Start()
    {   
        optiTrackDataDictionary = new Dictionary<int, OptitrackRBOriginData>();
        gameObjectDictionary = new Dictionary<int, TrackedItem>();

        _imageTargetHandler = _vufImageTarget.GetComponent<ImageTargetHandler>();
    }

    [PunRPC]
    public void ReceiveOptitrackRBState(object[] optitrackRBData)
    {
        Vector3 position = (Vector3)optitrackRBData[0];
        Quaternion rotation = (Quaternion)optitrackRBData[1];
        int id = (int)optitrackRBData[2];


        // Update the rigidbody dictionary with the appropriate data
        optiTrackDataDictionary[id] = new OptitrackRBOriginData(position, rotation);

        print("Received");
    }

    private TrackedItem CreateTrackedItem(int id, GameObject prefab)
    {
        return new TrackedItem()
        {
            Id = id,
            OptiTrackData = new OptiTrackData
            {
                Position = optiTrackDataDictionary[id].position,
                Rotation = optiTrackDataDictionary[id].rotation
            },
            TrackedObject = Instantiate(prefab, _optiTrackItemsParent)
        };
    }

    void VisualizeRigidbodies()
    {
        if (!_imageTargetHandler.IsImageTargetFound) return;

        // Send each of the rigidbody states
        foreach (int id in optiTrackDataDictionary.Keys)
        {
            // If the corresponding object is not already created, then create it
            // CREATION CODE
            if (!gameObjectDictionary.ContainsKey(id))
            {
                //Debug.Log($"creating id: {id}");

                switch (id)
                {
                    // Needle - special case for id=0 as it's center
                    case NEEDLE_ID:
                        gameObjectDictionary[id] = CreateTrackedItem(id, _ctrScanPrefab);
                        break;
                    // Phantom - use center as parent
                    case PHANTOM_ID:
                        gameObjectDictionary[id] = CreateTrackedItem(id, _needlePrefab);
                        break;
                    default:
                        break;
                }

            }
            else
            {
                //Debug.Log($"updating id: {id}");

                gameObjectDictionary[id].OptiTrackData.Position = optiTrackDataDictionary[id].position;
                gameObjectDictionary[id].OptiTrackData.Rotation = optiTrackDataDictionary[id].rotation;

                if (_imageTargetHandler.IsImageTargetFound)
                {

                    if (gameObjectDictionary.Count > 0)
                    {
                        if (id == 0)
                        {
                            // do nothing - keep it zeros
                        }
                        else
                        {
                            gameObjectDictionary[id].TrackedObject.transform.localPosition = optiTrackDataDictionary[id].position;
                            gameObjectDictionary[id].TrackedObject.transform.localRotation = optiTrackDataDictionary[id].rotation;
                        }

                    }
                }
            }
        }
    }


    private void Update()
    {
        VisualizeRigidbodies();
    }
}

class OptiTrackData
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
}

class TrackedItem
{
    public int Id { get; set; }

    public OptiTrackData OptiTrackData { get; set; }

    public GameObject TrackedObject { get; set; }

}

class TrackedItemList
{
    private Dictionary<int, TrackedItem> All;
}
