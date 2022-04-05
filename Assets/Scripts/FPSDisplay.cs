using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] Text fpsDisplay = null;

    private void Update()
    {
        fpsDisplay.text = (1f / Time.deltaTime).ToString("0.0");
    }
}
