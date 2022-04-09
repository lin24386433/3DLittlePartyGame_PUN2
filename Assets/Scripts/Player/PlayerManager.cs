using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{
	GameObject playerObj = null;

	public int KillsAmount = 0;
	public int DeathAmount = 0;
	public int PointAmount = 0;

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
