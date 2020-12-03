using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class OffWriter
{

    public static void SaveMesh(Mesh mesh, string savePath)
    {
        var format = new NumberFormatInfo();
        format.NumberDecimalSeparator = ".";
        format.NumberDecimalDigits = 10;
        format.NegativeSign = "-";

        StreamWriter outputFile = new StreamWriter(savePath);
        outputFile.WriteLine("OFF");

        if (mesh.triangles.Length % 3 != 0)
            Debug.LogError("Incorrect number of triangles");

        outputFile.WriteLine(mesh.vertexCount + " " + mesh.triangles.Length / 3 + " 0");


        foreach (Vector3 vertex in mesh.vertices)
        {
            outputFile.WriteLine(vertex.x.ToString(format) + " " + vertex.y.ToString(format) + " " + vertex.z.ToString(format));
        }

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            outputFile.WriteLine("3 " + mesh.triangles[i] + " " + mesh.triangles[i + 1] + " " + mesh.triangles[i + 2]);

        }

        outputFile.Close();
    }

    public static void WriteFile(string savePath, List<Vector3> listVertex, List<int> listIndices, List<int> listEdges)
    {
        var format = new NumberFormatInfo();
        format.NegativeSign = "-";
        format.NumberDecimalSeparator = ".";
        format.NumberDecimalDigits = 18;

        StreamWriter outputFile = new StreamWriter(savePath);
        outputFile.WriteLine("OFF");
        outputFile.WriteLine(listVertex.Count + " " + listIndices.Count / 3 + " " + listEdges.Count);

        foreach (Vector3 vertex in listVertex)
        {
            outputFile.WriteLine(vertex.x.ToString(format) + " " + vertex.y.ToString(format) + " " + vertex.z.ToString(format));
        }

        for (int i = 0; i <= listIndices.Count - 1; i += 2)
        {
            outputFile.WriteLine("3 " + listIndices[i] + " " + listIndices[i + 1] + " " + listIndices[i + 2]);
        }
    }

}
