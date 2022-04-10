using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBall : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private MeshRenderer meshRenderer = null;
    [SerializeField]
    private Collider skillCollider = null;

    [SerializeField]
    SkillSO[] skillSOs = null;

    [SerializeField]
    Vector4 randomSpawnBoundary = Vector4.zero;

    [SerializeField]
    Vector2 randomResetTimeBoundary = Vector2.zero;

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
    }

    private void RespawnSelf()
    {
        SetSelfActive(true);

        float randomX = Random.Range(randomSpawnBoundary.x, randomSpawnBoundary.y);
        float randomZ = Random.Range(randomSpawnBoundary.z, randomSpawnBoundary.w);

        photonView.RPC(nameof(SetPositionRPC), RpcTarget.All, randomX, transform.position.y, randomZ);
    }

    [PunRPC]
    void SetPositionRPC(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
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

        if (other.TryGetComponent<PlayerSkillHandler>(out PlayerSkillHandler playerSkillHandler))
        {
            int randomSkillSOIndex = Random.Range(0, skillSOs.Length);

            bool isAddSuccessfully = playerSkillHandler.AddSkill(skillSOs[randomSkillSOIndex], skillSOs[randomSkillSOIndex].GiveAmount);

            if (isAddSuccessfully)
                SetSelfActive(false);
        }
    }
}
