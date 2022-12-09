
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIChasePlayerState : AIState
{
    float timer = 0.0f;
    public void Enter(AIAgent agent)
    {
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
            float distance = (agent.player.position - agent.agent.destination).sqrMagnitude;
            if(distance > agent.config.maxDistance) {
                agent.agent.destination = agent.player.position;
            }
            timer = agent.config.maxTime;
        }
    }
}
