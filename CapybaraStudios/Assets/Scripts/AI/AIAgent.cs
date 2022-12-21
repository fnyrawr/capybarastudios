using System;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateId initalState;
    [NonSerialized] public NavMeshAgent agent;
    public AIAgentConfig config;
    public UIHealthBar healthBar;
    public Transform player;
    [SerializeField] public Transform rotationTarget;
    [SerializeField] public AIWeapons weapons; 
    public PlayerStats playerStats;
    public AISensor sensor;
    private bool playerFound; 

    void Awake()
    {   
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar = GetComponentInChildren<UIHealthBar>();
        playerStats = GetComponent<PlayerStats>();
        sensor = GetComponent<AISensor>();
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIFindWeaponState());
        stateMachine.RegisterState(new AIAttackPlayerState());
        stateMachine.ChangeState(initalState);
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.OnCharacterSpawned += UpdatePlayer;
    }

    void Update()
    {
        if(player == null) return;
        stateMachine.Update();
    }

    private void UpdatePlayer(GameObject player) {
        stateMachine.ChangeState(initalState);
        this.player = player.transform;
    }
}
