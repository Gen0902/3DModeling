using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Geometry
{
    public class Cylinder : MonoBehaviour
    {
        public Material mat;

        public int height = 40;
        public int radius = 5;
        public int n = 5;

        int[] triangles;
        int k = 0;

        Vector3[] vertices;

        void Start()
        {
            gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
            gameObject.AddComponent<MeshRenderer>();
            gameObject.GetComponent<MeshRenderer>().material = mat;
            DrawCylinder();
        }

        public void DrawCylinder()
        {
            float x, y, z, theta;

            vertices = new Vector3[2 * n + 2];
            triangles = new int[6 * n + 6 * n]; //3 * 2 vertices per lateral face + 3 vertices per horizontal faces (*2)

            for (int i = 0; i < n; i++)
            {
                theta = i * 2 * (float)Math.PI / n;
                x = (float)(radius * Math.Cos(theta));
                y = height / 2;
                z = (float)(radius * Mathf.Sin(theta));

                vertices[i] = new Vector3(x, y, z);
                vertices[i + n] = new Vector3(x, -y, z);

                if (i < n - 1)
                {
                    AddTriangle(i, i + n + 1, i + n);
                    AddTriangle(i, i + 1, i + n + 1);
                }
            }
            //Close the cylinder
            AddTriangle(0, n, n - 1);
            AddTriangle(2 * n - 1, n - 1, n);

            //Add top and bottom
            vertices[vertices.Length - 2] = new Vector3(0, height / 2, 0);
            vertices[vertices.Length - 1] = new Vector3(0, -height / 2, 0);

            for (int i = 0; i < n - 1; i++)
            {
                AddTriangle(vertices.Length - 2, i + 1, i);
                AddTriangle(vertices.Length - 1, i + n, i + n + 1);
            }

            AddTriangle(vertices.Length - 2, 0, n - 1);
            AddTriangle(vertices.Length - 1, 2 * n - 1, n);


            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = triangles;

            gameObject.GetComponent<MeshFilter>().mesh = msh;
        }

        private void AddTriangle(int a, int b, int c)
        {
            triangles[k] = a;
            triangles[k + 1] = b;
            triangles[k + 2] = c;
            k = k + 3;
        }

    }

}
