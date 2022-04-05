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
		CreateController();
	}

	void CreateController()
	{
		playerObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), new Vector3(0f, 5f, 0f), Quaternion.identity, 0, new object[] { photonView.ViewID });
	}

	public void Die()
	{
		PhotonNetwork.Destroy(playerObj);
		CreateController();
	}
}
