using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableFloor : MonoBehaviour
{
    public int Index;
    [SerializeField] FloorSpawner _floorSpawner;

    public void Init(int index, FloorSpawner floorSpawner)
    {
        Index = index;
        _floorSpawner = floorSpawner;
    }

    [ContextMenu("DestroySelf")]
    public void DestroySelf()
    {
        _floorSpawner.DestroyChild(Index);
    }
}
