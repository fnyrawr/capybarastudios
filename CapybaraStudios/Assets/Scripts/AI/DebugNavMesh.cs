using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DebugNavMesh : MonoBehaviour
{
    public bool velocity;
    public bool desiredVelocity;
    public bool path;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void OnDrawGizmos() {
        if(velocity) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        if(desiredVelocity) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        if(path) {
            Gizmos.color = Color.black;
            var agentPath = agent.path;
            Vector3 prev = transform.position;
            foreach(var corner in agentPath.corners) {
                Gizmos.DrawLine(prev, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prev = corner;
            }
        }

    }
}
