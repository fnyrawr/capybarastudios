using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscalatorPro
{
    public class EscalatorPro : MonoBehaviour
    {
        [Header("Config")]
        public GameObject stairPrefab;
        public float stairOffsetX = -0.1f, stairOffsetY = -0.1f;
        public int step = 3, stepBefore = 1, stepAfter = 1;

        [Header("Animation"), Range(0, 1)]
        public float speed = 0.15f;

        public bool stopped, reversed;

        [Header("Visibility")]
        public bool disableCollider;
        public bool disableMesh;

        [HideInInspector]
        public Vector3[] destPoints = new Vector3[8];
        [HideInInspector]
        public float stairSpacing;

        public enum Destination
        {
            ToSlopeStartPoint = 0,
            ToSlopeEndPoint = 1,
            ToEndPoint = 2,
            ToEndPointDown = 3,
            ToSlopeEndPointReturn = 4,
            ToSlopeStartPointReturn = 5,
            ToStartPointDown = 6,
            ToSartPoint = 7
        }

        private Stair[] allStair;

        public Vector3 GetTargetPointFromDestination(Destination dest)
        {
            return destPoints[(int)dest];
        }

        void Start()
        {
            allStair = GetComponentsInChildren<Stair>();
            UpdateConnectedStair();
        }

        private void UpdateConnectedStair()
        {
            if (!reversed)
                for (int i = 0; i < allStair.Length; i++)
                {
                    if (i != allStair.Length - 1)
                        allStair[i].connectedStair = allStair[i + 1];
                    else
                        allStair[i].connectedStair = allStair[0];
                }
            else
                for (int i = allStair.Length - 1; i > -1; i--)
                {
                    if (i != 0)
                        allStair[i].connectedStair = allStair[i - 1];
                    else
                        allStair[i].connectedStair = allStair[allStair.Length - 1];
                }
        }

        public void SetReversed(bool reversed)
        {
            this.reversed = reversed;
            for (int i = 0; i < allStair.Length; i++)
            {
                var stair = allStair[i];
                stair.ReverseDirection();
            }
            UpdateConnectedStair();
        }

        void OnValidate()
        {
            if (step < 1)
                step = 1;

            if (stepBefore < 1)
                stepBefore = 1;

            if (stepAfter < 1)
                stepAfter = 1;
        }
    }
}