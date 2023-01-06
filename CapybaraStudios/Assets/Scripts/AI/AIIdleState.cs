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
        
        if(agent.config.aIBehaviour == AIBehaviour.Dummy) return;

        if(!agent.agent.hasPath) {
            if (!agent.agent.isOnNavMesh) return;
            agent.WalkRandom(new Vector3(UnityEngine.Random.Range(1,100f), UnityEngine.Random.Range(0, 0.39f), UnityEngine.Random.Range(1,100f)));
        }

        if(agent.config.aIBehaviour != AIBehaviour.Aggressiv) return;

        float dist = (agent.player.position - agent.transform.position).sqrMagnitude;
        if(dist <= agent.config.minSightDistance * agent.config.minSightDistance
         && !Physics.Linecast(agent.player.position, agent.transform.position, agent.sensor.occlusionLayers)) {
            agent.stateMachine.ChangeState(AIStateId.Chase);
        }
    }
}