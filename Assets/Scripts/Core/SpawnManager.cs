using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance = null;

    void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    Transform[] spawnPoints;

    public Transform GetRandomSpawnPoint()
    {
        int ramdomValue = Random.Range(0, spawnPoints.Length - 1);

        return spawnPoints[ramdomValue];
    }
}
