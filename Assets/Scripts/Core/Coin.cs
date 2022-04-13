using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Coin : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private MeshRenderer meshRenderer = null;
    [SerializeField]
    private Collider skillCollider = null;

    [SerializeField]
    private GameObject getCoinParticlePrefab = null;

    [SerializeField]
    Vector4 randomSpawnBoundary = Vector4.zero;

    [SerializeField]
    Vector2 randomResetTimeBoundary = Vector2.zero;

    [SerializeField]
    private float rotateSpeed = 1f;

    Hashtable customValue;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RespawnSelf();
            GamePlayManager.OnCountDown += RespawnSelf;
        }
    }

    private void OnDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GamePlayManager.OnCountDown -= RespawnSelf;
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (transform.position.y < 20f)
            {
                RespawnSelf();
            }
        }

        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    }

    private void RespawnSelf()
    {
        SetSelfActive(true);

        float randomX = Random.Range(randomSpawnBoundary.x, randomSpawnBoundary.y);
        float randomZ = Random.Range(randomSpawnBoundary.z, randomSpawnBoundary.w);

        photonView.RPC(nameof(RespawnSelfRPC), RpcTarget.All, randomX, 23.5f, randomZ);
    }

    [PunRPC]
    void RespawnSelfRPC(float x, float y, float z)
    {
        transform.SetParent(null);

        transform.position = new Vector3(x, y, z);
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        GamePlayManager.Instance.CoinOwner = null;
    }

    void SetSelfActive(bool active)
    {
        photonView.RPC(nameof(SetSelfActiveRPC), RpcTarget.All, active);
    }

    [PunRPC]
    void SetSelfActiveRPC(bool active)
    {
        meshRenderer.enabled = active;
        skillCollider.enabled = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GamePlayManager.Instance.State != GamePlayManager.GameState.Gaming) return;

        if (transform.parent != null) return;

        if (other.TryGetComponent<PlayerModel>(out PlayerModel playerModel))
        {
            transform.SetParent(other.transform);
            transform.localPosition = Vector3.up * 3.5f;

            GamePlayManager.Instance.CoinOwner = playerModel.photonView.Owner.NickName;

            Instantiate(getCoinParticlePrefab, other.transform.position - new Vector3(0f, 2f, 0f), other.transform.rotation);
        }
    }
}
