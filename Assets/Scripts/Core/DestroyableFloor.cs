using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableFloor : MonoBehaviour
{
    public int Index;
    [SerializeField] FloorManager floorManager;

    public void Setup(int index, FloorManager floorManager)
    {
        Index = index;
        this.floorManager = floorManager;
    }

    [ContextMenu("DestroySelf")]
    public void DestroySelf()
    {
        floorManager.DestroyFloor(Index);
    }
}
