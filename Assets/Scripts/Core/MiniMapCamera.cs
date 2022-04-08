using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField]
    private Transform followTrans = null;

    [SerializeField]
    private float height = 10f;

    private void LateUpdate()
    {
        this.transform.position = new Vector3(followTrans.position.x, followTrans.position.y + height, followTrans.position.z);
        this.transform.rotation = Quaternion.Euler(new Vector3(90, followTrans.rotation.eulerAngles.y, transform.rotation.z));
    }
}
