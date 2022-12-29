using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackPlayerState : AIState
{
    private Vector3 oldPlayerPos = Vector3.zero;
    private float currentTime = 0f; 
    private Quaternion currRotation;
    private Quaternion lookRotation;
    private float timer = 0f;
    public void Enter(AIAgent agent)
    {
        currRotation = agent.transform.rotation;
        currentTime = agent.config.rotationSeconds;
        agent.agent.stoppingDistance = agent.config.stoppingDistance;
        agent.agent.destination = agent.transform.position;
        //agent.agent.
        //Rotate(agent);
    }

    public void Exit(AIAgent agent)
    {
      agent.weapons.SetFiring(false);
      agent.agent.stoppingDistance = 0;
    }

    public AIStateId GetId()
    {
       return AIStateId.AttackPlayer;
    }

    public void Update(AIAgent agent)
    {
        agent.weapons.Reload();

        if(currentTime < agent.config.rotationSeconds) {
            currentTime += Time.deltaTime;
            float percentage = currentTime / agent.config.rotationSeconds;
            currRotation = Quaternion.Lerp(currRotation, lookRotation, Mathf.SmoothStep(0, 1, percentage));
            Vector3 rotation = currRotation.eulerAngles;
            agent.rotationTarget.localRotation = Quaternion.Euler(rotation.x, 0f,0f);
            agent.transform.rotation = Quaternion.Euler(0f,rotation.y,0f);
            if(percentage >= 0.8f) {
                agent.weapons.SetFiring(true);
            }
        } else {
            currentTime = 0f;
            Vector3 targetDir = agent.target.position - agent.transform.position;
            if(Vector3.Angle(agent.target.position, agent.transform.position) > 20f) agent.weapons.SetFiring(false);
            lookRotation = Quaternion.LookRotation(targetDir);
            oldPlayerPos = agent.target.position;
        }

        //wenn player out of range geht, dann wechsle zum chase modus
        timer -= Time.deltaTime;
        if(timer < 0.0f) {
            float distance = (agent.player.position - agent.transform.position).sqrMagnitude;
            if(distance > agent.config.outOfRangeDistance * agent.config.outOfRangeDistance) {
                agent.stateMachine.ChangeState(AIStateId.Idle);
            } else if(distance > agent.config.maxDistance * agent.config.maxDistance) {
                agent.agent.destination = agent.player.position;
            }
            timer = agent.config.maxTime;
        }
    }    
}