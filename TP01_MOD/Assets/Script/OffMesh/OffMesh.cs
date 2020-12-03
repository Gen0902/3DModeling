using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffMesh
{
    public class OffMesh
    {
        public List<Vector3> vertices;
        public List<OffFace> faces;
        public List<Vector2> edges;

        public OffMesh()
        {
            vertices = new List<Vector3>();
            faces = new List<OffFace>();
            edges = new List<Vector2>();
        }

        public List<int> GetTriangles()
        {
            List<int> triangles = new List<int>();
            foreach (OffFace face in faces)
            {
                triangles.Add(face.verticesIndex[0]);
                triangles.Add(face.verticesIndex[1]);
                triangles.Add(face.verticesIndex[2]);
            }
            return triangles;
        }

        public void Recenter()
        {
            Vector3 G = new Vector3();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] *= 10;
                G += vertices[i];
            }

            G = G / vertices.Count;

            for (int i = 0; i < vertices.Count; i++)
                vertices[i] -= G;
        }

        public Vector3 ComputeGravityCenter()
        {
            Vector3 G = new Vector3();
            foreach (Vector3 v in vertices)
            {
                G += v;
            }
            G = G / vertices.Count;
            return G;
        }
    }

    public class OffFace
    {
        public int verticesCount;
        public int[] verticesIndex;

        public OffFace(int _verticesCount)
        {
            verticesCount = _verticesCount;
            verticesIndex = new int[verticesCount];
        }
    }
}