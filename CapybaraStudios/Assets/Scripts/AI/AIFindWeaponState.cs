using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindWeaponState : AIState
{
    public void Enter(AIAgent agent)
    {
        Weapon pickup = FindWeapon(agent);
        agent.agent.destination = pickup.transform.position;
        agent.agent.stoppingDistance = 0;
    }

    public void Exit(AIAgent agent)
    {
        agent.agent.stoppingDistance = 5;
    }

    public AIStateId GetId()
    {
        return AIStateId.FindWeapon;
    }

    public void Update(AIAgent agent)
    {

    }

    private Weapon FindWeapon(AIAgent agent) {
        Weapon[] weapons = Object.FindObjectsOfType<Weapon>();
        Weapon closest = null;
        float closestDistance = float.MaxValue;
        foreach (var weapon in weapons) {
            float distance = Vector3.Distance(agent.transform.position, weapon.transform.position);
            if(distance < closestDistance) {
                distance = closestDistance;
                closest = weapon;
            }
        }
        return closest;
    }
}
