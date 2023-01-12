using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    
    Vector3 impact = Vector3.zero;
    CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        // apply impact force:
        if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime);
        // consumes impact energy each frame:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        // reflect down force on the ground
        if (dir.y < 0) dir.y = -dir.y; 
        impact += dir.normalized * force / 3f;
    }
}
