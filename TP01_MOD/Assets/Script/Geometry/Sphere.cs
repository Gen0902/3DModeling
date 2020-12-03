using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Geometry
{
    public class Sphere : MonoBehaviour
    {
        public Material mat;

        [Range(1, 100)]
        public int radius = 5;
        [Range(1, 50)]
        public int p = 40;

        public int P { get { return p - 1; } }

        [Range(3, 50)]
        public int m = 5;

        List<int> triangles;

        void Start()
        {
            gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
            gameObject.AddComponent<MeshRenderer>();
            gameObject.GetComponent<MeshRenderer>().material = mat;
            DrawTriangles2(DrawSphere());
        }

        public Vector3[] DrawSphere()
        {
            List<Vector3> vertices = new List<Vector3>();
            float x, y, z, theta, phi;

            //Vector3[] vertices = new Vector3[m * p + 2];
            //triangles = new int[6 * m * p + 3 * m];

            for (int i = 0; i <= p; i++)
            {
                phi = i * (float)Math.PI / p;
                for (int j = 0; j <= m; j++)
                {
                    theta = j * 2 * (float)Math.PI / (float)m;
                    x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                    y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                    z = radius * Mathf.Cos(phi);

                    vertices.Add(new Vector3(x, y, z));
                }
            }

            ////Pole nord
            //x = (float)(radius * Math.Sin(0) * Math.Cos(0));
            //y = (float)(radius * Math.Sin(0) * Math.Sin(0));
            //z = (float)(radius * Math.Cos(0));
            //vertices.Add(new Vector3(x, y, z));

            ////Pole nord
            //x = (float)(radius * Math.Sin(Math.PI) * Math.Cos(Math.PI));
            //y = (float)(radius * Math.Sin(Math.PI) * Math.Sin(Math.PI));
            //z = (float)(radius * Math.Cos(Math.PI));
            //vertices.Add(new Vector3(x, y, z));


            return vertices.ToArray();
        }

        private void DrawTriangles(Vector3[] vertices)
        {
            triangles = new List<int>();
            for (int i = 0; i < p; i++)
            {
                for (int j = 0; j < m - 1; j++)
                {
                    if (j + 1 + m * (i + 1) < m * P)
                    {
                        AddTriangle(j + m * i, j + m * (i + 1), j + 1 + m * (i + 1));
                        AddTriangle(j + m * i, j + 1 + m * (i + 1), j + m * i + 1);
                    }
                }
            }
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = triangles.ToArray();
            gameObject.GetComponent<MeshFilter>().mesh = msh;
        }

        private void DrawTriangles2(Vector3[] vertices)
        {
            triangles = new List<int>();
            int k1, k2;
            for (int i = 0; i < p; i++)
            {
                k1 = i * (m + 1);     // beginning of current stack
                k2 = k1 + m + 1;      // beginning of next stack

                for (int j = 0; j < m; j++, k1++, k2++)
                {
                    if (i != 0)   //Si on est au nord
                        AddTriangle(k1, k2, k1 + 1);

                    if (i != (p - 1)) //Si on est au sud
                        AddTriangle(k1 + 1, k2, k2 + 1);
                }
            }
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = triangles.ToArray();
            gameObject.GetComponent<MeshFilter>().mesh = msh;
        }

        private void AddTriangle(int a, int b, int c)
        {
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);
        }

        private void OnDrawGizmos()
        {
            Vector3[] vertices = DrawSphere();
            Gizmos.color = Color.black;
            foreach (var point in vertices)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}