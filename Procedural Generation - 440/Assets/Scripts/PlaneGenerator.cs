using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

//from: https://www.youtube.com/watch?v=-3ekimUWb9I
//ZeroKelvinTutorials video on 

public class PlaneGenerator : MonoBehaviour
{
    public Vector2 meshSize;
    public int sideSteps;
    public MeshFilter planeMesh;
    private Mesh curMesh;
    
    private void Start()
    {
        curMesh = new();
        planeMesh.mesh = curMesh;
        
        CreatePerlinPlane(meshSize, sideSteps);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CreateFlatPlane(meshSize, sideSteps);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CreatePerlinPlane(meshSize, sideSteps);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CreateRandomPlane(meshSize, sideSteps);
        }

    }

    public void AssignMesh(Mesh mesh, Vector3[] vertices, int[] triangles)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    public List<int> AssignTriangles(int sideLength)
    {
        var triangles = new List<int>();

        for (int row = 0; row < sideLength; row++)
        {
            for (int col = 0; col < sideLength; col++)
            {
                int index = row * sideLength + row + col;

                triangles.Add(index);
                triangles.Add(index + sideLength + 2);
                triangles.Add(index + sideLength + 1);

                triangles.Add(index);
                triangles.Add(index + 1);
                triangles.Add(index + sideLength + 2);
            }
        }

        return triangles;
    }

    public void CreateFlatPlane(Vector2 size, int sideLength)
    {
        var vertices = new List<Vector3>();

        float xStep = size.x / sideLength;
        float zStep = size.y / sideLength;

        for (int x = 0; x < sideLength + 1; x++)
        {
            for (int z = 0; z < sideLength + 1; z++)
            {
                Vector3 vertPos = new Vector3(x * xStep, 0, z * zStep);

                vertices.Add(vertPos);
            }
        }
        
        AssignMesh(planeMesh.mesh, vertices.ToArray(), AssignTriangles(sideLength).ToArray());
    }
    
    public void CreateRandomPlane(Vector2 size, int sideLength)
    {
        var vertices = new List<Vector3>();

        float xStep = size.x / sideLength;
        float zStep = size.y / sideLength;

        for (int x = 0; x < sideLength + 1; x++)
        {
            for (int z = 0; z < sideLength + 1; z++)
            {
                Vector3 vertPos = new Vector3(x * xStep, Random.Range(0f,1f), z * zStep);

                vertices.Add(vertPos);
            }
        }
        
        AssignMesh(planeMesh.mesh, vertices.ToArray(), AssignTriangles(sideLength).ToArray());
    }

    public void CreatePerlinPlane(Vector2 size, int sideLength)
    {
        var vertices = new List<Vector3>();

        float xStep = size.x / sideLength;
        float zStep = size.y / sideLength;

        for (int x = 0; x < sideLength + 1; x++)
        {
            for (int z = 0; z < sideLength + 1; z++)
            {
                float yHeight = Mathf.PerlinNoise((float)x + .07f, (float)z + 0.07f);
                Debug.Log(yHeight);

                Vector3 vertPos = new Vector3(x * xStep, yHeight, z * zStep);

                vertices.Add(vertPos);
            }
        }

        AssignMesh(planeMesh.mesh, vertices.ToArray(), AssignTriangles(sideLength).ToArray());
    }

    public float PerlinNoise(int x, int y)
    {
        uint temp = (uint) Mathf.Abs(x + y);

        temp >>= 3;
        temp <<= 5;

        float value = temp + temp / (1 + (temp * x + temp * y));

        return value;
    }
}
