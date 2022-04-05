using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance = null;
    public static MenuManager Instance => _instance;

    public GameObject[] menus = new GameObject[0];

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        SetMenu(MenuType.Start);
    }

    public void SetMenu(MenuType menuType)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if (i == (int)menuType)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }

    public enum MenuType
    {
        Load,
        Start,
        RoomList,
        CreateRoom,
        Room
    }
}
