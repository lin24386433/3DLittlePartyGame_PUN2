using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListCell : MonoBehaviour
{
	[SerializeField] TMP_Text roomNameTxt;

	public RoomInfo info;

	public void SetUp(RoomInfo _info)
	{
		info = _info;
		roomNameTxt.text = _info.Name;
	}

	public void OnClick()
	{
		RoomManager.Instance.JoinRoom(info);
	}
}
