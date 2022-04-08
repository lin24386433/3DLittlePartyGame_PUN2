using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] Text fpsDisplay = null;

    float lastUpdateTime = 0f;

    private void Update()
    {
        if (Time.time - lastUpdateTime >= .1f)
        {
            fpsDisplay.text = "FPS : " + (1f / Time.deltaTime).ToString("0.0");
            lastUpdateTime = Time.time;
        }
    }
}
