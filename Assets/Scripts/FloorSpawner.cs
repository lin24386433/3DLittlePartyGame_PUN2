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

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        for(int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                GameObject obj = Instantiate(floorObj, new Vector3(transform.position.x + i, transform.position.y, transform.position.z + j), transform.rotation, transform);

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
        foreach(Transform obj in transform)
        {
            DestroyImmediate(obj.gameObject);
        }
    }

    public void DestroyChild(int index)
    {
        photonView.RPC("DestroyChildRPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void DestroyChildRPC(int index)
    {
        Destroy(transform.GetChild(index).gameObject);
    }
}
