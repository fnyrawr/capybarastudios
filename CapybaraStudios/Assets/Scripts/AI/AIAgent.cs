using System;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateId initalState;
    [NonSerialized] public NavMeshAgent agent;
    public AIAgentConfig config;
    public UIHealthBar healthBar;
    public Transform player;
    void Awake()
    {   
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar = GetComponentInChildren<UIHealthBar>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIFindWeaponState());
        stateMachine.ChangeState(initalState);
    }

    void Update()
    {
        stateMachine.Update();
    }
}
