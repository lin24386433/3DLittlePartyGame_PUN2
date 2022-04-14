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
		if (!photonView.IsMine) return;

		GamePlayManager.OnCountDown += ResetPlayerPoints;

		CreateController();

		ResetPlayerPoints();
	}

    private void OnDestroy()
    {
		if (!photonView.IsMine) return;

		GamePlayManager.OnCountDown -= ResetPlayerPoints;
	}

	void ResetPlayerPoints()
    {
		Hashtable hashTable = new Hashtable();
		hashTable.Add("Kill", 0);
		hashTable.Add("Death", 0);
		hashTable.Add("Point", 0);
		photonView.Owner.SetCustomProperties(hashTable);
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
