using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
	GameObject playerObj = null;

	public bool isDead = false;

    private void Start()
    {
		if(photonView.IsMine)
			CreateController();
	}

	void CreateController()
	{
		var spawnPoint = SpawnManager.Instance.GetRandomSpawnPoint();
		
		playerObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { photonView.ViewID });
	}

	public void Die()
	{
		PhotonNetwork.Destroy(playerObj);
		CreateController();
	}
}
