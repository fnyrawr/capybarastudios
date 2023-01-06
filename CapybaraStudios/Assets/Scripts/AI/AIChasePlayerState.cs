using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIChasePlayerState : AIState
{
    float timer = 0.0f;
    public void Enter(AIAgent agent)
    {
        agent.agent.destination = agent.player.position;
        agent.agent.stoppingDistance = agent.config.stoppingDistance;
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AIStateId GetId()
    {
        return AIStateId.Chase;
    }

    public void Update(AIAgent agent)
    {
        if(!agent.enabled) return;

        if(!agent.agent.hasPath) agent.agent.destination = agent.player.position;

        timer -= Time.deltaTime;
        if(timer < 0.0f) {
            float distance = (agent.player.position - agent.transform.position).sqrMagnitude;
            if(distance > agent.config.outOfRangeDistance * agent.config.outOfRangeDistance) {
                agent.stateMachine.ChangeState(AIStateId.Idle);
            }
            else if(distance > agent.config.maxDistance * agent.config.maxDistance) {
                agent.agent.destination = agent.target.position;
            } else {
                agent.agent.destination = agent.transform.position;
                agent.stateMachine.ChangeState(AIStateId.AttackPlayer);
            }
            timer = agent.config.maxTime;
        }
    }
}