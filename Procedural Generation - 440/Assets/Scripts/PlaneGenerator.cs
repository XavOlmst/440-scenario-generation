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
        
        CreateRandomPlane(meshSize, sideSteps);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            CreateFlatPlane(meshSize, sideSteps);
        }
    }

    public void AssignMesh(Mesh mesh, Vector3[] vertices, int[] triangles)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
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
}
