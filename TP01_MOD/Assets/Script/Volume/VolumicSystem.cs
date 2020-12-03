using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Volume
{
    public class VolumicSystem : MonoBehaviour
    {
        [Header("Settings")]
        [Range(0.1f, 1)] public float cellSize = 0.2f;
        [Range(0.5f, 1)] public float sizeModifier = 1f;
        [Range(1, 5)] public int intersection = 1;

        [Header("Meta Balls")]
        public bool metaBalls = false;
        public bool update = true;
        [Range(0, 10)] public float materialFieldRange = 10;
      public int materialFieldThreshold = 5;

        [Header("OcTree")]
        public bool ocTree = false;
        [Range(1, 20)] public int maxDepth = 10;

        [Header("View")]
        public Color gizmosColor = Color.red;
        public bool showBox = false;
        public bool showWireSpheres = false;
        public bool showWireField = false;

        [Header("Spheres placing")]
        public Sphere[] spheres;

        private List<Cube> gizmoCubes;


        public void Enumerate(Box box)
        {
            for (int x = 0; x < box.NodesLength.x; x++)
            {
                for (int y = 0; y < box.NodesLength.y; y++)
                {
                    for (int z = 0; z < box.NodesLength.z; z++)
                    {
                        int presency = 0;
                        foreach (Sphere sphere in spheres)
                        {
                            if (PointInSphereRange(box.nodes[x, y, z].Position, sphere))
                            {
                                if (sphere.type == Sphere.EType.Fill)
                                    presency++;
                                else if (sphere.type == Sphere.EType.Extrusion)
                                {
                                    presency = 0;
                                    break;
                                }
                            }

                        }
                        if (presency >= intersection) Gizmos.DrawCube(box.nodes[x, y, z].Position, new Vector3(cellSize, cellSize, cellSize) * sizeModifier);
                    }
                }
            }
        }

        public void GenerateMetaBalls(Box box)
        {
            for (int x = 0; x < box.NodesLength.x; x++)
            {
                for (int y = 0; y < box.NodesLength.y; y++)
                {
                    for (int z = 0; z < box.NodesLength.z; z++)
                    {
                        int intersection = 0;
                        float potential = 0;

                        foreach (Sphere sphere in spheres)
                        {
                            if (PointInSphereRange(box.nodes[x, y, z].Position, sphere))
                            {
                                if (sphere.type == Sphere.EType.Fill)
                                    intersection++;
                                else if (sphere.type == Sphere.EType.Extrusion)
                                {
                                    intersection = 0;
                                    break;
                                }
                            }
                            else
                            {
                                float distance = (box.nodes[x, y, z].Position - sphere.center).magnitude;
                                if (distance <= materialFieldRange)
                                    potential += 1 - Mathf.Pow(0.444f * distance, 6) + Mathf.Pow(1.888f * distance, 4) - Mathf.Pow(2.444f * distance, 2);
                                //potential += Mathf.Clamp(sphere.materialFieldForce / distance, 0, 10000);
                            }

                        }
                        if (intersection >= this.intersection || potential >= materialFieldThreshold)
                        {
                            Cube c = new Cube(box.nodes[x, y, z].Position, new Vector3(cellSize, cellSize, cellSize) * sizeModifier);
                            gizmoCubes.Add(c);

                        }
                    }
                }
            }
        }

        public void StartEnumerateOcTree()
        {
            ocTree = true;
            Box box = Box.CreateBoundingBox(this.spheres, this.cellSize);
            ready = false;
            positions.Clear();
            sizes.Clear();
            EnumerateOcTree(box.Start, box.End, box.Size.x);
            ready = true;
        }
        public void EnumerateOcTree(Vector3 start, Vector3 end, float _cellSize, int depth = 0)
        {
            if (depth > maxDepth)
            {
                positions.Add(start + (end - start) / 2);
                sizes.Add(new Vector3(_cellSize, _cellSize, _cellSize));
                return;
            }

            int k = 0;
            foreach (Sphere sphere in spheres)
            {
                if (PointInSphereRange(start + new Vector3(0, 0, 0), sphere)) k++;
                if (PointInSphereRange(start + new Vector3(_cellSize, 0, 0), sphere)) k++;
                if (PointInSphereRange(start + new Vector3(0, _cellSize, 0), sphere)) k++;
                if (PointInSphereRange(start + new Vector3(0, 0, _cellSize), sphere)) k++;
                if (PointInSphereRange(start + new Vector3(_cellSize, _cellSize, 0), sphere)) k++;
                if (PointInSphereRange(start + new Vector3(_cellSize, 0, _cellSize), sphere)) k++;
                if (PointInSphereRange(start + new Vector3(0, _cellSize, _cellSize), sphere)) k++;
                if (PointInSphereRange(start + new Vector3(_cellSize, _cellSize, _cellSize), sphere)) k++;
            }
            if (k >= 8)
            {
                positions.Add(start + (end - start) / 2);
                sizes.Add(new Vector3(_cellSize, _cellSize, _cellSize));
                //Debug.Log("Adding cube : in pos :" + positions[positions.Count - 1] + "size : " + sizes[sizes.Count - 1]);
            }
            else if (k == 0 && depth != 0)
                return;
            else if (k != 8 || depth == 0)
            {
                depth++;
                Vector3 diag = end - start;
                _cellSize /= 2;
                //front bottom left
                EnumerateOcTree(start, start + diag / 2, _cellSize, depth);
                //front bottom middle
                EnumerateOcTree(start + new Vector3(diag.x / 2, 0, 0), start + new Vector3(diag.x, diag.y / 2, diag.z / 2), _cellSize, depth);
                //front middle left
                EnumerateOcTree(start + new Vector3(0, diag.y / 2, 0), start + new Vector3(diag.x / 2, diag.y, diag.z / 2), _cellSize, depth);
                //front middle middle
                EnumerateOcTree(start + new Vector3(diag.x / 2, diag.y / 2, 0), start + new Vector3(diag.x, diag.y, diag.z / 2), _cellSize, depth);
                //back bottom left
                EnumerateOcTree(start + new Vector3(0, 0, diag.z / 2), start + new Vector3(diag.x / 2, diag.y / 2, diag.z), _cellSize, depth);
                //back bottom middle
                EnumerateOcTree(start + new Vector3(diag.x / 2, 0, diag.z / 2), start + new Vector3(diag.x, diag.y / 2, diag.z), _cellSize, depth);
                //back middle left
                EnumerateOcTree(start + new Vector3(0, diag.y / 2, diag.z / 2), start + new Vector3(diag.x / 2, diag.y, diag.z), _cellSize, depth);
                //back middle middle
                EnumerateOcTree(start + diag / 2, end, _cellSize, depth);
            }

        }

        public bool PointInSphereRange(Vector3 point, Sphere sphere)
        {
            Vector3 v = point - sphere.center;
            float diff = v.x * v.x + v.y * v.y + v.z * v.z - sphere.radius * sphere.radius;
            if (diff <= 0) return true;
            else return false;
        }

        bool ready = false;
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> sizes = new List<Vector3>();
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (Sphere sphere in spheres)
            {
                if (showWireSpheres) Gizmos.DrawWireSphere(sphere.center, sphere.radius);
                if (showWireField) Gizmos.DrawWireSphere(sphere.center, materialFieldRange);
            }
            Box box = Box.CreateBoundingBox(this.spheres, this.cellSize);
            if (showBox) Gizmos.DrawWireCube(box.Start + (box.End - box.Start) / 2f, box.Size);


            Gizmos.color = gizmosColor;
            if (ocTree && ready)
            {
                metaBalls = false;
                for (int i = 0; i < positions.Count; i++)
                    Gizmos.DrawCube(positions[i], sizes[i] * sizeModifier);
            }
            else if (metaBalls)
            {
                ocTree = false;
                ready = false;
                if (update)
                {
                    gizmoCubes = new List<Cube>();
                    GenerateMetaBalls(box);
                }
                foreach (Cube cube in gizmoCubes)
                    Gizmos.DrawCube(cube.position, cube.size);
            }
            else
                Enumerate(box);
        }
    }
}



//public void EnumerateOcTree(Vector3 start, Vector3 end, float _cellSize, int depth = 0)
//{
//    Box box = new Box(start, end, _cellSize);
//    if (depth == 0)
//    {
//        box = new Box(box.Start, box.End, box.Size.x);
//        EnumerateOcTree(box.Start, box.End, ++depth);
//        return;
//    }

//    for (int x = 0; x < box.NodesLength.x; x++)
//    {
//        for (int y = 0; y < box.NodesLength.y; y++)
//        {
//            for (int z = 0; z < box.NodesLength.z; z++)
//            {
//                _cellSize /= 2f;
//                EnumerateOcTree(box.nodes[x, y, z].Position, box.nodes[x + 1, y + 1, z + 1].Position, _cellSize);
//                EnumerateOcTree(start + new Vector3(diag.x / 2, 0, 0), start + diag / 2 + new Vector3(diag.x / 2, 0, 0));
//                EnumerateOcTree(start + new Vector3(0, 0, diag.x / 2), start + diag / 2 + new Vector3(diag.x / 2, 0, 0));
//                EnumerateOcTree(start, start + diag / 2);
//                EnumerateOcTree(start, start + diag / 2);
//                EnumerateOcTree(start, start + diag / 2);
//                EnumerateOcTree(start, start + diag / 2);
//                EnumerateOcTree(start, start + diag / 2);


//                for (int i = 1; i <= 2; i++)
//                {
//                    for (int j = 1; j <= 2; j++)
//                    {
//                    }
//                }

//                int presency = 0;
//                foreach (Sphere sphere in spheres)
//                {
//                    {
//                        if (PointInSphereRange(new Vector3(x, y, z), sphere))
//                            if (sphere.type == Sphere.EType.Fill)
//                                presency++;
//                            else if (sphere.type == Sphere.EType.Extrusion)
//                            {
//                                presency = 0;
//                                break;
//                            }
//                    }
//                }
//                if (presency >= intersection) Gizmos.DrawCube(new Vector3(x, y, z), new Vector3(cellSize, cellSize, cellSize) * sizeModifier);

//            }
//        }

//        if (true)
//        {

//        }
//    }

//}
