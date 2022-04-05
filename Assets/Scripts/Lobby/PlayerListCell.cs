using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListCell : MonoBehaviourPunCallbacks
{
	[SerializeField] TMP_Text text;
	[SerializeField] GameObject hostImage;
	Player player;

	public void SetUp(Player _player)
	{
		player = _player;
		text.text = _player.NickName;

		hostImage.SetActive(player.IsMasterClient);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (player == otherPlayer)
		{
			Destroy(gameObject);
		}
		hostImage.SetActive(player.IsMasterClient);
	}

	public override void OnLeftRoom()
	{
		Destroy(gameObject);
	}
}
