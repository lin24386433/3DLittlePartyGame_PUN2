using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private MeshRenderer meshRenderer = null;
    [SerializeField]
    private Collider skillCollider = null;

    [SerializeField]
    Vector4 randomSpawnBoundary = Vector4.zero;

    [SerializeField]
    Vector2 randomResetTimeBoundary = Vector2.zero;

    [SerializeField]
    private float rotateSpeed = 1f;

    float timer = 0f;
    float randomResetTime = 0f;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            randomResetTime = Random.Range(randomResetTimeBoundary.x, randomResetTimeBoundary.y);
            RespawnSelf();
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timer += Time.deltaTime;

            if (timer >= randomResetTime)
            {
                timer = 0f;
                RespawnSelf();
                randomResetTime = Random.Range(randomResetTimeBoundary.x, randomResetTimeBoundary.y);
            }
        }

        transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    }

    private void RespawnSelf()
    {
        SetSelfActive(true);

        float randomX = Random.Range(randomSpawnBoundary.x, randomSpawnBoundary.y);
        float randomZ = Random.Range(randomSpawnBoundary.z, randomSpawnBoundary.w);

        photonView.RPC(nameof(SetTranformRPC), RpcTarget.All, randomX, transform.position.y, randomZ);
    }

    [PunRPC]
    void SetTranformRPC(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
        transform.rotation = Quaternion.Euler(90, 0, Random.Range(0, 360));
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

        if (other.TryGetComponent<PlayerModel>(out PlayerModel playerModel))
        {
            playerModel.AddPoints(1);
            SetSelfActive(false);
        }
    }
}
