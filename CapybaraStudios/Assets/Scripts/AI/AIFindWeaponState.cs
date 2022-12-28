using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFindWeaponState : AIState
{
    Weapon pickup;
    GameObject[] weapons = new GameObject[1];
    public void Enter(AIAgent agent)
    {
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

        pickup = FindWeapon(agent);
        if(pickup != null) agent.agent.destination = pickup.transform.position;

        //walk random
        if(!agent.agent.hasPath) {
            if (!agent.agent.isOnNavMesh) return;
            agent.WalkRandom(new Vector3(UnityEngine.Random.Range(1f,100f), UnityEngine.Random.Range(0, 0.39f), UnityEngine.Random.Range(1,100f)));
        }
    }

    private Weapon FindWeapon(AIAgent agent) {
        int count = agent.sensor.FilterByTag(weapons, "Weapon");
        if(count > 0) {
            return weapons[0].GetComponent<Weapon>();
        }
        return null;
    }
}