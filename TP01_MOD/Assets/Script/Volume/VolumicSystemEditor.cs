using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Volume
{
    [CustomEditor(typeof(VolumicSystem))]
    public class VolumicSystemEditor : Editor
    {
        public VolumicSystem system;

        private void OnEnable()
        {
            system = (VolumicSystem)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Start OcTree"))
                system.StartEnumerateOcTree();
        }
    }
}
