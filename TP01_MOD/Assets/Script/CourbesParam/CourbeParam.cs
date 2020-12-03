using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourbeParam : MonoBehaviour
{
    public Transform[] curve1;
    public Transform[] curve2;
    public float timeStep = 0.01f;

    Vector2 Bezier(Transform[] points, float t)
    {
        Vector2 Q = Vector2.zero;
        int n = points.Length;

        for (int i = 0; i < n; i++)
        {
            Q += (Vector2)points[i].position * Bernstein(i, n - 1, t);
        }

        return Q;
    }

    float Bernstein(int i, int n, float u)
    {
        return factorial(n) / (factorial(i) * factorial(n - i)) * Mathf.Pow(u, i) * Mathf.Pow(1 - u, n - i);
    }

    public int factorial(int f)
    {
        if (f == 0)
            return 1;
        else
            return f * factorial(f - 1);
    }

    void AdjustPoints()
    {
        Transform pa = curve1[curve1.Length - 2];
        Transform pb = curve1[curve1.Length - 1];
        Transform pc = curve2[1];

        Vector3 dir = pc.position - pa.position;
        Vector3 middle = pa.position + dir * 0.5f;

        Vector3 dirToAlign = pb.position - middle;
        pa.position += dirToAlign; 
        pc.position += dirToAlign; 
    }

    private void OnDrawGizmos()
    {
        curve2[0].position = curve1[curve1.Length - 1].position;
        AdjustPoints();
        for (int i = 0; i < curve1.Length - 1; i++)
        {
            Gizmos.DrawLine(curve1[i].position, curve1[i + 1].position);
        }
        for (int i = 0; i < curve2.Length - 1; i++)
        {
            Gizmos.DrawLine(curve2[i].position, curve2[i + 1].position);
        }

        Gizmos.color = Color.red;
        Vector2 lastPoint = curve1[0].position;
        for (float t = 0; t <= 1; t += timeStep)
        {
            Vector2 point = Bezier(curve1, t);
            Gizmos.DrawLine(lastPoint, point);
            lastPoint = point;
        }

        Gizmos.color = Color.green;
        lastPoint = curve2[0].position;
        for (float t = 0; t <= 1; t += timeStep)
        {
            Vector2 point = Bezier(curve2, t);
            Gizmos.DrawLine(lastPoint, point);
            lastPoint = point;
        }
    }
}
