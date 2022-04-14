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

    List<DestroyableFloor> floorsToDestroy = new List<DestroyableFloor>();

    [SerializeField] float timeToDestroyFloors = .1f;
    float timer = 0f;

    private void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        rigi.velocity = transform.TransformDirection(Vector3.forward * speed);

        if (!photonView.IsMine) return;

        timer += Time.deltaTime;

        if (timer > timeToDestroyFloors)
        {
            timer = 0f;

            if (floorsToDestroy.Count == 0) return;

            int[] floorsToDestroyIndexs = new int[floorsToDestroy.Count];
            for(int i = 0; i < floorsToDestroy.Count; i++)
            {
                floorsToDestroyIndexs[i] = floorsToDestroy[i].Index;
            }

            FloorManager.Instace.DestroyFloors(floorsToDestroyIndexs);

            floorsToDestroy.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<DestroyableFloor>(out DestroyableFloor hittedFloor))
        {
            if(!floorsToDestroy.Contains(hittedFloor))
                floorsToDestroy.Add(hittedFloor);
        }
    }
}
