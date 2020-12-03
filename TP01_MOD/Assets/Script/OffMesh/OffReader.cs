using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Data;
using System.Globalization;

namespace OffMesh
{
    public class OffReader
    {
        public static OffMesh ReadFile(string path)
        {
            FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //BufferedStream bs = new BufferedStream(fs);
            StreamReader sr = new StreamReader(fs);

            int nbVertices, nbFaces, nbEdges;
            string line;

            if (sr.ReadLine() != "OFF")
            {
                Debug.LogError("File does not start with \"OFF\" line !");
                return null;
            }

            string s = sr.ReadLine();
            string[] parameters = s.Split(' ');
            if (parameters.Length != 3)
            {
                Debug.LogError("Wrong parameters format line 2");
                return null;
            }

            nbVertices = int.Parse(parameters[0]);
            nbFaces = int.Parse(parameters[1]);
            nbEdges = int.Parse(parameters[2]);

            OffMesh mesh = new OffMesh();


            for (int i = 0; i < nbVertices; i++)
            {
                line = sr.ReadLine();
                float[] pos = ParseVertex(line);
                mesh.vertices.Add(new Vector3(pos[0], pos[1], pos[2]));
            }

            for (int i = 0; i < nbFaces; i++)
            {
                line = sr.ReadLine();
                OffFace face = ParseFace(line);
                mesh.faces.Add(face);
            }

            //int[] edge = ParseEdge(line);
            //mesh.edges.Add(new Vector2(edge[0], edge[1]));
            return mesh;
        }

        private static float[] ParseVertex(string line)
        {
            try
            {
                float[] position = new float[3];
                string[] vertex = line.Split(' ');

                NumberFormatInfo format = new NumberFormatInfo();
                format.NumberDecimalSeparator = ".";
                format.NumberDecimalDigits = 10;
                format.NegativeSign = "-";

                position[0] = float.Parse(vertex[0], format);
                position[1] = float.Parse(vertex[1], format);
                position[2] = float.Parse(vertex[2], format);
                return position;
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format($"Can't find coordinates in line : {0}. ERROR : {1}", line, ex));
                return null;
            }
        }

        private static OffFace ParseFace(string line)
        {
            try
            {
                string[] data = line.Split(' ');
                int nbVtx = int.Parse(data[0]);
                OffFace face = new OffFace(nbVtx);

                for (int i = 0; i < nbVtx; i++)
                {
                    face.verticesIndex[i] = int.Parse(data[i + 1]);
                }
                return face;
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format($"Can't find face data in line : {0}. ERROR : {1}", line, ex));
                return null;
            }
        }

        private static int[] ParseEdge(string line)
        {
            try
            {
                string[] data = line.Split(' ');
                return new int[] { int.Parse(data[0]), int.Parse(data[1]) };
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format($"Can't find edge data in line : {0}. ERROR : {1}", line, ex));
                return null;
            }
        }
    }

}