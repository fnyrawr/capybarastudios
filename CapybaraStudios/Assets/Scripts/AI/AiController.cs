using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiController : MonoBehaviour
{
    public Transform player;
    NavMeshAgent agent;
    Animator animator;
    Ragdoll ragdoll;
    private int _forwardBackwardHash;
    [SerializeField]
    private float maxHealth = 100;
    private float currHealth;
    //Time
    float timer = 0.0f;
    public float maxTime = 1f;
    public float maxDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        _forwardBackwardHash = Animator.StringToHash("movementForwards");
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f) {
            float distance = (player.position-agent.destination).sqrMagnitude;
            if(distance > maxDistance) {
                agent.destination = player.position;
            }
            timer = maxTime;
        }
        animator.SetFloat(_forwardBackwardHash, agent.velocity.magnitude);
    }

    private void Damage(float amount) {
        currHealth -= amount;
        if(currHealth <= 0f) {
            ragdoll.EnablePhysics();
        }
    } 
}
