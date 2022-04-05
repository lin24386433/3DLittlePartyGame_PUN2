using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableFloor : MonoBehaviour
{
    [SerializeField] FloorSpawner floorSpawner;

    private void Start()
    {
        floorSpawner = transform.GetComponentInParent<FloorSpawner>();
    }

    [ContextMenu("DestroySelf")]
    public void DestroySelf()
    {
        floorSpawner.DestroyChild(transform.GetSiblingIndex());
    }
}
