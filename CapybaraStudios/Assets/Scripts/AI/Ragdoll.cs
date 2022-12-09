using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rBodies;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DeactivatePhysics();
    }

    private void DeactivatePhysics() {
        foreach(var r in rBodies) {
            r.isKinematic = true;
        }
    }

    public void EnablePhysics() {
        foreach(var r in rBodies) {
            r.isKinematic = false;
        }
        animator.enabled = false;
    }
}
