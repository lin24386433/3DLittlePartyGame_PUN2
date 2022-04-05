using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter = null;
    [SerializeField] MeshCollider meshCollider = null;
    Mesh mesh;

    Vector3[] vertices;

    int[] triangles;

    private void Start()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        CreateMesh();
        UpdateMesh();
    }

    private void CreateMesh()
    {
        vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1)
        };

        triangles = new int[]
        {
            0, 1 ,2,
            1, 3, 2
        };
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
