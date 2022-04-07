using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private Rigidbody rigi = null;

    private void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        rigi.velocity = transform.TransformDirection(Vector3.forward * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<DestroyableFloor>(out DestroyableFloor hittedFloor))
        {
            hittedFloor.DestroySelf();
        }
    }
}
