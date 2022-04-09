using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviourPunCallbacks
{
    public static FloorManager Instace = null;

    [SerializeField] List<DestroyableFloor> destroyableFloors = new List<DestroyableFloor>();
    List<int> destroyedFloors = new List<int>();

    private void Awake()
    {
        if (Instace == null)
            Instace = this;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        RecoverDestroyedFloor();
    }

    private void RecoverDestroyedFloor()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int randomRecoverCount = UnityEngine.Random.Range(1, destroyedFloors.Count / 15);

        List<int> recoverFloorIndexList = new List<int>();

        for (int i = 1; i <= randomRecoverCount; i++)
        {
            if(destroyedFloors.Count >= i)
            {
                recoverFloorIndexList.Add(destroyedFloors[i - 1]);
            }
        }

        RecoverFloors(recoverFloorIndexList.ToArray());

        Invoke(nameof(RecoverDestroyedFloor), UnityEngine.Random.Range(2f, 10f));
    }

    #region - DestroyFloor -
    public void DestroyFloor(int index)
    {
        if (GamePlayManager.Instance.State != GamePlayManager.GameState.Gaming) return;

        photonView.RPC(nameof(DestroyFloorRPC), RpcTarget.All, index);
    }

    [PunRPC]
    public void DestroyFloorRPC(int index)
    {
        destroyedFloors.Add(index);
        destroyableFloors[index].gameObject.SetActive(false);
    }

    public void DestroyFloors(int[] indexArray)
    {
        if (GamePlayManager.Instance.State != GamePlayManager.GameState.Gaming) return;

        photonView.RPC(nameof(DestroyFloorsRPC), RpcTarget.All, indexArray);
    }

    [PunRPC]
    public void DestroyFloorsRPC(int[] indexArray)
    {
        for(int i = 0; i < indexArray.Length; i++)
        {
            destroyedFloors.Add(indexArray[i]);
            destroyableFloors[indexArray[i]].gameObject.SetActive(false);
        }
    }

    #endregion

    #region - RecoverFloor - 

    public void RecoverFloor(int index)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC(nameof(RecoverFloorRPC), RpcTarget.All, index);
    }

    [PunRPC]
    public void RecoverFloorRPC(int index)
    {
        if (!destroyedFloors.Contains(index)) return;

        destroyedFloors.Remove(index);
        destroyableFloors[index].gameObject.SetActive(true);
    }

    public void RecoverFloors(int[] indexArray)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC(nameof(RecoverFloorsRPC), RpcTarget.All, indexArray);
    }

    [PunRPC]
    public void RecoverFloorsRPC(int[] indexArray)
    {
        for (int i = 0; i < indexArray.Length; i++)
        {
            if (!destroyedFloors.Contains(indexArray[i])) continue;

            destroyedFloors.Remove(indexArray[i]);
            destroyableFloors[indexArray[i]].gameObject.SetActive(true);
        }
    }
    #endregion

    #region - Editor Only -
    [ContextMenu("SetupAllDestroyableFloor")]
    public void SetupAllDestroyableFloor()
    {
        destroyableFloors.Clear();

        int index = 0;

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<DestroyableFloor>(out DestroyableFloor childFloor))
            {
                childFloor.Setup(index, this);

                destroyableFloors.Add(childFloor);

                index++;
            }
        }
    }
    #endregion
}
