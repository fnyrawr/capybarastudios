using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscalatorPro
{
    public class Stair : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody body;
        [HideInInspector]
        public Stair connectedStair;
        [HideInInspector]
        public EscalatorPro.Destination nextDestination;
        private EscalatorPro escalator;
        private MeshRenderer mesh;
        private Collider col;

        private Vector3 targetPos;

        void Start()
        {
            body = GetComponent<Rigidbody>();
            escalator = transform.parent.GetComponent<EscalatorPro>();
            mesh = GetComponent<MeshRenderer>();
            col = GetComponent<Collider>();

            if (escalator.reversed)
            {
                ReverseDirection();
            }

            BeginTween();
        }

        private void HandleVisibility()
        {
            if (!escalator.reversed)
            {
                if (nextDestination == EscalatorPro.Destination.ToSlopeEndPointReturn ||
                            nextDestination == EscalatorPro.Destination.ToSlopeStartPointReturn)
                {
                    if (escalator.disableMesh && mesh)
                        mesh.enabled = false;

                    if (escalator.disableCollider && col)
                        col.enabled = false;
                }

                if (nextDestination == EscalatorPro.Destination.ToStartPointDown ||
                nextDestination == EscalatorPro.Destination.ToSartPoint)
                {
                    if (escalator.disableMesh && mesh)
                        mesh.enabled = true;

                    if (escalator.disableCollider && col)
                        col.enabled = true;
                }
            }
            else
            {
                if (nextDestination == EscalatorPro.Destination.ToSlopeEndPointReturn ||
                nextDestination == EscalatorPro.Destination.ToSlopeEndPointReturn)
                {
                    if (escalator.disableMesh && mesh)
                        mesh.enabled = false;

                    if (escalator.disableCollider && col)
                        col.enabled = false;
                }

                if (nextDestination == EscalatorPro.Destination.ToEndPointDown ||
                nextDestination == EscalatorPro.Destination.ToEndPoint)
                {
                    if (escalator.disableMesh && mesh)
                        mesh.enabled = true;

                    if (escalator.disableCollider && col)
                        col.enabled = true;
                }
            }
        }

        void FixedUpdate()
        {
            HandleVisibility();
            if (escalator.GetTargetPointFromDestination(nextDestination) == transform.localPosition)
            {
                if (!escalator.reversed)
                {
                    if ((int)nextDestination < 7)
                        nextDestination = (EscalatorPro.Destination)((int)nextDestination + 1);
                    else
                        nextDestination = (EscalatorPro.Destination)0;
                }
                else
                {
                    //The escalator is reversed!
                    ReverseDirection();
                }

                BeginTween();
            }

            if (escalator.stopped) return;

            var calculatedPos = transform.parent.TransformPoint(Vector3.MoveTowards(body.transform.localPosition, targetPos, escalator.speed * Time.deltaTime * 3));
            body.MovePosition(calculatedPos);
        }

        private void BeginTween()
        {
            targetPos = escalator.GetTargetPointFromDestination(nextDestination);
        }

        public void ReverseDirection()
        {
            if ((int)nextDestination > 0)
                nextDestination = (EscalatorPro.Destination)((int)nextDestination - 1);
            else
                nextDestination = (EscalatorPro.Destination)7;
        }
    }
}