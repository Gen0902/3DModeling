using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class SimplifierManager : MonoBehaviour
{
    public bool simplify = false;
    public float cellSize;
    public MeshFilter originalModel;
    public MeshFilter newModel;

    Mesh originalMesh;
    Mesh simplifiedMesh;

    private Dictionary<Vector3Int, Cell> grid;
    private Vector3Int gridStart = Vector3Int.zero;
    private Vector3Int gridEnd = Vector3Int.zero;


    private void Update()
    {
        if (simplify)
        {
            Simplify();
            simplify = false;
        }
    }

    private void Simplify()
    {
        originalMesh = originalModel.mesh;
        simplifiedMesh = new Mesh();

        grid = BuildGrid();
        Debug.Log("Grid built.");
        RecalculateVertices();
        Debug.Log("Vertices calculated.");
        RecalculateTriangles();
        Debug.Log("Triangles calculated.");

        newModel.mesh = simplifiedMesh;
    }

    private Dictionary<Vector3Int, Cell> BuildGrid()
    {
        Dictionary<Vector3Int, Cell> newGrid = new Dictionary<Vector3Int, Cell>();
        int index = 0;
        for (int i = 0; i < originalMesh.vertices.Length; i++)
        {
            Vector3Int coord = new Vector3Int(Mathf.FloorToInt(originalMesh.vertices[i].x / cellSize),
                Mathf.FloorToInt(originalMesh.vertices[i].y / cellSize),
                Mathf.FloorToInt(originalMesh.vertices[i].z / cellSize));

            UpdateBoundingBox(coord);

            if (!newGrid.ContainsKey(coord))
                newGrid.Add(coord, new Cell() { index = index++, vertices = new List<Vector3>() { originalMesh.vertices[i] } });
            else
                newGrid[coord].vertices.Add(originalMesh.vertices[i]);
        }
        return newGrid;
    }

    void RecalculateTriangles()
    {
        List<int> newTriangles = new List<int>();

        for (int i = 0; i < originalMesh.triangles.Length; i += 3)
        {
            int indexA = GetCellIndex(originalMesh.vertices[originalMesh.triangles[i]]);
            int indexB = GetCellIndex(originalMesh.vertices[originalMesh.triangles[i + 1]]);
            int indexC = GetCellIndex(originalMesh.vertices[originalMesh.triangles[i + 2]]);

            if (!IsSameCell(indexA, indexB, indexC))
            {
                newTriangles.Add(indexA);
                newTriangles.Add(indexB);
                newTriangles.Add(indexC);
            }
        }

        simplifiedMesh.triangles = newTriangles.ToArray();
    }
    private int GetCellIndex(Vector3 position)
    {
        Vector3Int coord = new Vector3Int(Mathf.FloorToInt(position.x / cellSize),
           Mathf.FloorToInt(position.y / cellSize),
           Mathf.FloorToInt(position.z / cellSize));

        return grid[coord].index;
    }

    private bool IsSameCell(int indexA, int indexB, int indexC)
    {
        if (indexA == indexB || indexB == indexC || indexC == indexA)
            return true;

        return false;
    }

    private void RecalculateVertices()
    {
        List<Vector3> newVertices = new List<Vector3>();

        foreach (KeyValuePair<Vector3Int, Cell> cell in grid)
        {
            Vector3 averagePos = Vector3.zero;
            int count = 0;
            foreach (Vector3 vertex in cell.Value.vertices)
            {
                averagePos += vertex;
                count++;
            }
            averagePos /= count;
            newVertices.Add(averagePos);
        }

        simplifiedMesh.vertices = newVertices.ToArray();
    }

    private void UpdateBoundingBox(Vector3Int newPosition)
    {
        if (newPosition.x < gridStart.x) gridStart.x = newPosition.x;
        if (newPosition.y < gridStart.y) gridStart.y = newPosition.y;
        if (newPosition.z < gridStart.z) gridStart.z = newPosition.z;

        if (newPosition.x > gridEnd.x) gridEnd.x = newPosition.x;
        if (newPosition.y > gridEnd.y) gridEnd.y = newPosition.y;
        if (newPosition.z > gridEnd.z) gridEnd.z = newPosition.z;
    }
}

public struct Cell
{
    public int index;
    public List<Vector3> vertices;
}