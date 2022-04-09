using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    [SerializeField] GameObject floorObj = null;
    [SerializeField] int maxX, minX, maxY, minY;

    [SerializeField] Material floorMaterial = null;
    [SerializeField] Material floorMaterial1 = null;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                GameObject obj = Instantiate(floorObj, new Vector3(transform.position.x + i, transform.position.y, transform.position.z + j), transform.rotation, transform);

                if (i % 2 == 0)
                {
                    obj.GetComponent<MeshRenderer>().material = j % 2 == 0 ? floorMaterial : floorMaterial1;
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = j % 2 == 0 ? floorMaterial1 : floorMaterial;
                }
            }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        DestroyableFloor[] children = GetComponentsInChildren<DestroyableFloor>();

        foreach(DestroyableFloor child in children)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}
