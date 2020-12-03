using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Geometry
{
    public class Triangle : MonoBehaviour
    {
        public Material mat;

        [Range(0, 100)]
        public int width = 1;
        [Range(0, 100)]
        public int height = 1;

        int savedWidth = 0;
        int savedHeight = 0;

        // Use this for initialization
        void Start()
        {
            gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
            gameObject.AddComponent<MeshRenderer>();
            gameObject.GetComponent<MeshRenderer>().material = mat;
        }

        private void Update()
        {
            if (width != savedWidth || height != savedHeight)
            {
                savedWidth = width;
                savedHeight = height;
                DrawVertices();
            }
        }

        public void DrawVertices()
        {
            int[] triangles = new int[6 * height * width];
            Vector3[] vertices = new Vector3[6 * height * width];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int i = (x + y * width) * 6;
                    vertices[i] = new Vector3(x, y, 0);
                    vertices[i + 1] = new Vector3(x + 1, y, 0);
                    vertices[i + 2] = new Vector3(x, y + 1, 0);
                    vertices[i + 3] = new Vector3(x + 1, y + 1, 0);

                    triangles[i] = i;
                    triangles[i + 1] = i + 1;
                    triangles[i + 2] = i + 2;

                    triangles[i + 3] = i + 1;
                    triangles[i + 4] = i + 3;
                    triangles[i + 5] = i + 2;
                }
            }

            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = triangles;

            gameObject.GetComponent<MeshFilter>().mesh = msh;
        }

    }
}