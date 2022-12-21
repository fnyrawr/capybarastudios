using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFindWeaponState : AIState
{
    Weapon pickup;
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
        pickup = FindWeapon(agent);
        if(pickup != null) agent.agent.destination = pickup.transform.position;

        //get RandomPath
        if(!agent.agent.hasPath) {
            Vector3 randomDirection = new Vector3(Random.Range(1,100), Random.Range(0, 0.39f), Random.Range(1,100));
            randomDirection += agent.transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, 5, 1)) {
                finalPosition = hit.position;            
            }
            agent.agent.destination = finalPosition;
        }

        if(agent.weapons.HasWeapon()) {
            agent.stateMachine.ChangeState(AIStateId.Idle);
        }
    }

    private Weapon FindWeapon(AIAgent agent) {
        if(agent.sensor.objects.Count > 0) {
            return agent.sensor.objects[0].GetComponent<Weapon>();
        }
        return null;
    }
}
