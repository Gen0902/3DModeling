using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OffMesh
{
    public class MeshLoader : MonoBehaviour
    {
        public string fileName;
        public Material mat;
        public bool save = false;

        // Start is called before the first frame update
        void Start()
        {
            string path = $@"Assets\{fileName}.off";
            Mesh mesh = new Mesh();
            OffMesh offMesh = OffReader.ReadFile(path);
            offMesh.Recenter();
            mesh.vertices = offMesh.vertices.ToArray();
            mesh.triangles = offMesh.GetTriangles().ToArray();
            mesh.RecalculateNormals();

            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            Debug.Log("Mesh - vertices : " + mesh.vertices.Length + " triangles : " + mesh.triangles.Length / 3.0f);

            if (save)
                OffWriter.SaveMesh(mesh, $@"D:\Documents\DAMBRAINE_JEROME\Unity\TP01_MOD\Assets\{fileName}.off");
        }


    }
}
