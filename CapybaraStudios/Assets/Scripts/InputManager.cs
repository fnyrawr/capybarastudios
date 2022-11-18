using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.WalkingActions walking;
    public PlayerInput.ShootingActions shooting;

    private PlayerMovement movement;
    private GunScript gun;
    private PlayerLook look;

    Coroutine fireCoroutine;

    void Awake()
    {
        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        shooting = playerInput.Shooting;

        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        gun = GetComponent<GunScript>();
        //
        walking.Jump.performed += ctx => movement.Jump();

        walking.Crouch.performed += ctx => {
            movement.Crouch();
            look.Crouch();
        }; 
        walking.Sprint.performed += ctx => {
            movement.Sprint();
            look.Sprint();
        };

        //shooting.Shoot.performed += ctx => gun.Shoot();
        shooting.Shoot.started += ctx => StartFiring();
        shooting.Shoot.canceled += ctx => StopFiring();

        shooting.Reload.performed += ctx => gun.Reload();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move with PlayerMovement by the value of the movement action
        movement.ProcessMove(walking.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(walking.LookAround.ReadValue<Vector2>());
    }

    //walking
    private void OnEnable()
    {
        walking.Enable();
        shooting.Enable();
    }

    private void OnDisable()
    {
        walking.Disable();
        shooting.Disable();
    }

    //For Rapid Fire
    void StartFiring()
    {
        fireCoroutine = StartCoroutine(gun.RapidFire());
    }
    void StopFiring()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

}