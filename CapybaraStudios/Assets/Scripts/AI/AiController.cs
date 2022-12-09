using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiController : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    Ragdoll ragdoll;
    private int _forwardBackwardHash;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        _forwardBackwardHash = Animator.StringToHash("movementForwards");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(_forwardBackwardHash, agent.velocity.magnitude);
    }
}
