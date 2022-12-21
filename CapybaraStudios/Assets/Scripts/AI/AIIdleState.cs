using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    private float healTimer = 0f;
    public void Enter(AIAgent agent)
    {    
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateId GetId()
    {
        return AIStateId.Idle;
    }

    public void Update(AIAgent agent)
    {   
        healTimer += Time.deltaTime;
        if(healTimer > agent.config.healTime) {
            agent.playerStats.Heal(1);
            healTimer = 0f;
        }
        Vector3 pdir = agent.player.position - agent.transform.position;
        if(pdir.magnitude > agent.config.maxSightDistance) {
            return;
        }

        Vector3 adir = agent.transform.forward;

        float dot = Vector3.Dot(pdir.normalized, adir);
        if(dot > 0.0f) {
            agent.stateMachine.ChangeState(AIStateId.Chase);
        }
    }
}
