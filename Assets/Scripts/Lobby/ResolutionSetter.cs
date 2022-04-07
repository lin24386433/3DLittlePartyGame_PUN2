using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    public bool IsFullScreen = false;

    public void SetFullScreen(bool isFullScreen)
    {
        IsFullScreen = isFullScreen;
        Screen.fullScreen = IsFullScreen;
        PlayerPrefs.SetInt("isFullScreen", IsFullScreen ? 1 : 0);
    }

    public void SetResolution(int resolutions)
    {
        PlayerPrefs.SetInt("Resolution", resolutions);
        switch (resolutions)
        {
            case 0:
                Screen.SetResolution(3840, 2160, IsFullScreen);
                break;
            case 1:
                Screen.SetResolution(2560, 1440, IsFullScreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, IsFullScreen);
                break;
            case 3:
                Screen.SetResolution(1600, 900, IsFullScreen);
                break;
            case 4:
                Screen.SetResolution(1280, 720, IsFullScreen);
                break;
            case 5:
                Screen.SetResolution(960, 540, IsFullScreen);
                break;
        }
    }
}
