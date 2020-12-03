using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class Geometry : MonoBehaviour
{
    public Material mat;

    public int height = 20;
    public int radius = 5;
    public int n = 5;


    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;
        DrawCylinder();
    }

    public void DrawCylinder()
    {
        float theta = 20;

        float x;
        float y;
        float z;

        int[] triangles = new int[6 * n];
        Vector3[] vertices = new Vector3[6 * n];

        int i = 0;
        for (int k = 0; k < n; k++)
        {
            theta = i * 2 * (float)Math.PI / n;
            x = (float)(radius * Math.Cos(theta));
            y = -height / 2;
            z = (float)(radius * Math.Sin(theta));

            vertices[i] = new Vector3(x, y, z);
            i++;
        }

        for (int k = 0; k < n; k++)
        {
            theta = i * 2 * (float)Math.PI / n;
            x = (float)(radius * Math.Cos(theta));
            y = height / 2;
            z = (float)(radius * Math.Sin(theta));

            vertices[i] = new Vector3(x, y, z);
            i++;
        }

        for (int index = 0; index < vertices.Length; i++)
        {
            if (index + n < vertices.Length)
            {
                triangles[index] = index;
                triangles[index + 1] = index + n;
                triangles[index + 2] = index + n + 1;
            }
        }

        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;
    }

}

