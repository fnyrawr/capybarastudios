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

        float dist = Vector3.Distance(agent.player.position, agent.transform.position);
        Vector3 dir = (agent.player.position - agent.transform.position).normalized;
        //RAYCAST soll enemy hitten
        Debug.DrawRay(agent.player.position, dir * 20f, Color.blue, 20f);
        RaycastHit hit;
        Physics.Raycast(agent.transform.position, dir * agent.config.minSightDistance, out hit);
        Debug.Log(hit.transform.gameObject.tag);
        if(dist <= agent.config.minSightDistance && hit.transform.gameObject.tag == "Enemy") {
            agent.stateMachine.ChangeState(AIStateId.Chase);
        }
    }
}
