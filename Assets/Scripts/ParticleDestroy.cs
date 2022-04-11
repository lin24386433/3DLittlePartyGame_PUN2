using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    [SerializeField]
    float destroyTime = 1f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
