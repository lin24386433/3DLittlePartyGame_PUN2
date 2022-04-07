using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject floorObj = null;
    [SerializeField] int maxX, minX, maxY, minY;

    [SerializeField] Material floorMaterial = null;
    [SerializeField] Material floorMaterial1 = null;

    [SerializeField] List<DestroyableFloor> destroyableFloors;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                ResetAllFloor();
            }
        }
    }

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        int index = 0;
        destroyableFloors = new List<DestroyableFloor>();

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                GameObject obj = Instantiate(floorObj, new Vector3(transform.position.x + i, transform.position.y, transform.position.z + j), transform.rotation, transform);

                var floor = obj.GetComponent<DestroyableFloor>();
                floor.Init(index, this);
                index++;
                destroyableFloors.Add(floor);

                if (i % 2 == 0)
                {
                    obj.GetComponent<MeshRenderer>().material = j % 2 == 0 ? floorMaterial : floorMaterial1;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = j % 2 == 0 ? floorMaterial1 : floorMaterial;
                }
            }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        DestroyableFloor[] children = GetComponentsInChildren<DestroyableFloor>();

        foreach(DestroyableFloor child in children)
        {
            DestroyImmediate(child.gameObject);
        }

        destroyableFloors.Clear();
        destroyableFloors = null;
    }

    public void DestroyChild(int index)
    {
        photonView.RPC("DestroyChildRPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void DestroyChildRPC(int index)
    {
        for(int i = 0; i < destroyableFloors.Count; i++)
        {
            if(destroyableFloors[i].Index == index)
            {
                if (destroyableFloors[i].gameObject != null)
                    destroyableFloors[i].gameObject.SetActive(false);
            }
        }
    }

    [ContextMenu("ResetAllFloor")]
    public void ResetAllFloor()
    {
        photonView.RPC("ResetAllFloorRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ResetAllFloorRPC()
    {
        for (int i = 0; i < destroyableFloors.Count; i++)
        {
            destroyableFloors[i].gameObject.SetActive(true);
        }
    }
}
