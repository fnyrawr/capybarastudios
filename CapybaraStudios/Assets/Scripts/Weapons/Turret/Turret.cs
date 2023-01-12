using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public Transform body;

    private void Update() 
    {
        Aim();
    }

    private void Aim()
    {
        float targetPlaneAngle = vector3AngleOnPlane(target.position, transform.position, -body.transform.up, body.transform.forward);
        Vector3 newRotation = new Vector3(0, targetPlaneAngle, 0);
        body.transform.Rotate(newRotation, Space.Self);
    }

    float vector3AngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal, Vector3 toZeroAngle)
    {
        Vector3 projectedVector = Vector3.ProjectOnPlane(from - to, planeNormal);
        float projectedVectorAngle = Vector3.SignedAngle(projectedVector, toZeroAngle, planeNormal);

        return projectedVectorAngle;
    } 
}
