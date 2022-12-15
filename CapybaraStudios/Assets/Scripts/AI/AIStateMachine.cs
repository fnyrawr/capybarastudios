using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    private AIState[] states;
    private AIAgent agent;
    private AIStateId currentState;

    public AIStateMachine(AIAgent agent) {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateId)).Length;
        states = new AIState[numStates];
    }

    public void RegisterState(AIState state) {
        int index = (int) state.GetId();
        states[index] = state;
    }

    public AIState GetState(AIStateId id) {
        int idx = (int) id;
        return states[idx];
    }
    public void Update() {
        GetState(currentState).Update(agent);
    }

    public void ChangeState(AIStateId nState) {
        GetState(currentState).Exit(agent);
        currentState = nState;
        GetState(currentState).Enter(agent);
    }
}
