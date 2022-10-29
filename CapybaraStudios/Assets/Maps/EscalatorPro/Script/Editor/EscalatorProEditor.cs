using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EscalatorPro
{
    [CustomEditor(typeof(EscalatorPro))]
    public class EscalatorProEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var escalatorPro = target as EscalatorPro;

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stopped"));
                EditorGUILayout.HelpBox("Other config was disabled in play mode", MessageType.Warning);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            if (escalatorPro.stairPrefab == null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stairPrefab"));
                EditorGUILayout.HelpBox("Please select a stair prefab first!", MessageType.Warning);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            DrawDefaultInspector();

            var originalBgColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Generate"))
            {
                Generate(escalatorPro);
            }
            GUI.backgroundColor = originalBgColor;
        }

        private void Generate(EscalatorPro escalatorPro)
        {
            ClearChildern(escalatorPro);

            var prefabSize = escalatorPro.stairPrefab.GetComponent<MeshRenderer>().bounds.size;
            var stairSpacing = prefabSize.x + escalatorPro.stairOffsetX;
            var stairHeight = prefabSize.y + escalatorPro.stairOffsetY;

            escalatorPro.stairSpacing = stairSpacing;

            for (int i = 0; i < escalatorPro.stepBefore; i++)
            {
                var pos = new Vector3(i * stairSpacing, 0, 0);
                CreateStair(escalatorPro, pos, i == escalatorPro.stepBefore - 1 ?
                    (EscalatorPro.Destination)1 : (EscalatorPro.Destination)0
                );

                if (i == 0)
                {
                    escalatorPro.destPoints[7] = pos;
                }

                if (i == escalatorPro.stepBefore - 1)
                {
                    escalatorPro.destPoints[0] = pos;
                }
            }

            for (int i = 0; i < escalatorPro.step; i++)
            {
                var pos = new Vector3((i + escalatorPro.stepBefore) * stairSpacing, (i + 1) * stairHeight, 0);
                CreateStair(escalatorPro, pos, (EscalatorPro.Destination)1);
            }

            for (int i = 0; i < escalatorPro.stepAfter; i++)
            {
                var pos = new Vector3((i + escalatorPro.step + escalatorPro.stepBefore) * stairSpacing, (escalatorPro.step + 1) * stairHeight, 0);
                CreateStair(escalatorPro, pos, i == escalatorPro.stepAfter - 1 ?
                    (EscalatorPro.Destination)3 : (EscalatorPro.Destination)2
                );

                if (i == 0)
                {
                    escalatorPro.destPoints[1] = pos;
                }

                if (i == escalatorPro.stepAfter - 1)
                {
                    escalatorPro.destPoints[2] = pos;
                }
            }

            //Returning stairs


            for (int i = escalatorPro.stepAfter - 1; i > -1; i--)
            {
                //Returning stair
                var pos2 = new Vector3((i + escalatorPro.step + escalatorPro.stepBefore) * stairSpacing, (escalatorPro.step + 1) * stairHeight, 0);
                pos2.y -= stairSpacing;
                CreateStair(escalatorPro, pos2, i == 0 ?
                    (EscalatorPro.Destination)5 : (EscalatorPro.Destination)4
                );

                if (i == 0)
                {
                    escalatorPro.destPoints[4] = pos2;
                }

                if (i == escalatorPro.stepAfter - 1)
                {
                    escalatorPro.destPoints[3] = pos2;
                }
            }

            for (int i = escalatorPro.step - 1; i > -1; i--)
            {
                //Returning stair
                var pos2 = new Vector3((i + escalatorPro.stepBefore) * stairSpacing, (i + 1) * stairHeight, 0);
                pos2.y -= stairSpacing;
                CreateStair(escalatorPro, pos2, (EscalatorPro.Destination)5);
            }

            for (int i = escalatorPro.stepBefore - 1; i > -1; i--)
            {
                //Returning stair
                var pos2 = new Vector3(i * stairSpacing, 0, 0);
                pos2.y -= stairSpacing;
                CreateStair(escalatorPro, pos2, i == 0 ?
                    (EscalatorPro.Destination)7 : (EscalatorPro.Destination)6
                );

                if (i == 0)
                {
                    escalatorPro.destPoints[6] = pos2;
                }

                if (i == escalatorPro.stepBefore - 1)
                {
                    escalatorPro.destPoints[5] = pos2;
                }
            }
        }

        public void ClearChildern(EscalatorPro escalatorPro)
        {
            foreach (var item in escalatorPro.GetComponentsInChildren<Transform>())
            {
                if (item != escalatorPro.transform)
                    DestroyImmediate(item.gameObject);
            }
        }

        private void CreateStair(EscalatorPro escalatorPro, Vector3 pos, EscalatorPro.Destination dest)
        {
            var stair = PrefabUtility.InstantiatePrefab(escalatorPro.stairPrefab) as GameObject;
            stair.transform.SetParent(escalatorPro.transform);
            stair.transform.localPosition = pos;

            if (!stair.GetComponent<Rigidbody>())
            {
                var body = stair.AddComponent<Rigidbody>();
                body.useGravity = false;
                body.isKinematic = true;
                body.interpolation = RigidbodyInterpolation.None;
            }

            if (!stair.GetComponent<Stair>())
            {
                var s = stair.AddComponent<Stair>();
                s.nextDestination = dest;
            }
        }
    }
}