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
        Weapon[] weapons = Object.FindObjectsOfType<Weapon>();
        Weapon closest = null;
        float closestDistance = float.MaxValue;
        Debug.Log(weapons.Length);
        foreach (var weapon in weapons) {
            //wenn BoxCollider false ist, dann hat jemand anderes schon die waffe
            if(weapon.GetComponent<BoxCollider>().enabled == false) continue;
            float distance = Vector3.Distance(agent.transform.position, weapon.transform.position);
            if(distance < closestDistance) {
                closestDistance = distance;
                closest = weapon;
            }
        }
        return closest;
    }
}
