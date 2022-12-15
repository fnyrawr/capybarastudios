using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateId {
    Chase, 
    Death,
    Idle,
    FindWeapon,
    AttackPlayer
}

public interface AIState
{
    AIStateId GetId();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
