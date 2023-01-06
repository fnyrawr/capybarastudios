using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public void Enter(AIAgent agent)
    {
        agent.healthBar.gameObject.SetActive(false);
        agent.weapons.DitchWeapon();
        //agent.weapons.enabled = false;
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateId GetId()
    {
        return AIStateId.Death;
    }

    public void Update(AIAgent agent)
    {
    }
}
