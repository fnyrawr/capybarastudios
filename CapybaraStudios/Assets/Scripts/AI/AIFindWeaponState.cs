using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindWeaponState : AIState
{
    Weapon pickup;
    public void Enter(AIAgent agent)
    {
        pickup = FindWeapon(agent);
        agent.agent.destination = pickup.transform.position;
        agent.agent.stoppingDistance = 0;
    }

    public void Exit(AIAgent agent)
    {
    }

    public AIStateId GetId()
    {
        return AIStateId.FindWeapon;
    }

    public void Update(AIAgent agent)
    {
        if(agent.weapons.HasWeapon()) {
            agent.stateMachine.ChangeState(AIStateId.Idle);
        }
        if(pickup.GetComponent<BoxCollider>().enabled == false) {
            pickup = FindWeapon(agent);
            agent.agent.destination = pickup.transform.position;
        }
    }

    private Weapon FindWeapon(AIAgent agent) {
        if(agent.sensor.objects.Count > 0) {
            return agent.sensor.objects[0].GetComponent<Weapon>();
        }
        return null;
    }
}
