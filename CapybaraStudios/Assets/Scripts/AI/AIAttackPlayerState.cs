using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackPlayerState : AIState
{
    public void Enter(AIAgent agent)
    {
        
    }

    public void Exit(AIAgent agent)
    {
      
    }

    public AIStateId GetId()
    {
       return AIStateId.AttackPlayer;
    }

    public void Update(AIAgent agent)
    {
        //wenn player out of range geht, dann wechsle zum chase modus
        agent.transform.LookAt(agent.player.transform.position);
    }
}
