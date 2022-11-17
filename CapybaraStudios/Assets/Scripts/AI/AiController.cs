using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiController : MonoBehaviour
{
    public Transform player;
    NavMeshAgent agent;
    Animator animator;

    private int _forwardBackwardHash;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        _forwardBackwardHash = Animator.StringToHash("movementForwards");
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.position;
        animator.SetFloat(_forwardBackwardHash, agent.velocity.magnitude);
    }
}
