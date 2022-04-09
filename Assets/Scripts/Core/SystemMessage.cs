using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemMessage : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup = null;
    [SerializeField]
    private TMP_Text messageTxt = null;

    [SerializeField]
    private float displayTime = 3f;

    float timer = 0;

    public void ShowMessages(string messages)
    {
        canvasGroup.alpha = 1;
        messageTxt.text = messages;

        timer = displayTime;
    }

    private void LateUpdate()
    {
        timer -= Time.deltaTime;

        canvasGroup.alpha = Mathf.Lerp(1f, 0f, 1f - (timer / displayTime));
    }
}
