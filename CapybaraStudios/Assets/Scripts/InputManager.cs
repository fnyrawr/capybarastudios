using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
public class InputManager : NetworkBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.WalkingActions walking;
    public PlayerInput.ShootingActions shooting;

    private PlayerMovement movement;
    //private GunScript gun; 
    private PlayerLook look;

    public override void OnNetworkSpawn()
    {
            if (!IsClient || !IsOwner)
            {
                enabled = false;
                // dont need to do anything else if not the owner
                return;
            }
    }

    public void Awake()
    {

        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        movement = GetComponent<PlayerMovement>();
        //shooting = playerInput.Shooting;
        //look = GetComponent<PlayerLook>();
        //gun = GetComponent<GunScript>();
        //shooting.Shoot.performed += ctx => gun.Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        //move with PlayerMovement by the value of the movement action
        movement.ProcessMove(walking.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        /*look.ProcessLook(walking.LookAround.ReadValue<Vector2>());*/
    }

    private void OnEnable()
    {
        walking.Jump.performed += ctx => movement.Jump();
        walking.Crouch.performed += ctx => movement.Crouch();
        walking.Sprint.performed += ctx => movement.Sprint();
        walking.Enable();
        //shooting.Enable();
    }
    private void OnDisable()
    {
        //walking.Jump.performed -= ctx => movement.Jump();
        walking.Disable();
        //shooting.Disable();
    }
}
