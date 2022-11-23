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
    private SpecificWeaponScript specifcWeapon;

    Coroutine fireCoroutine;

    void Awake()
    {
        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        shooting = playerInput.Shooting;

        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        gun = GetComponent<GunScript>();
        if (GetComponentInChildren<SpecificWeaponScript>() != null)
        {
            specifcWeapon = GetComponentInChildren<SpecificWeaponScript>();
            Debug.Log("SpecificWeaponScript gefunden");
        }
            
        //

        walking.Jump.performed += ctx => movement.Jump();

        walking.Crouch.performed += ctx =>
        {
            movement.Crouch();
            look.Crouch();
        };
        walking.Sprint.performed += ctx =>
        {
            movement.Sprint();
            look.Sprint();
        };

        shooting.Shoot.started += ctx => StartFiring();
        shooting.Shoot.canceled += ctx => StopFiring();

        //shooting.Reload.performed += ctx => gun.Reload();
        //shooting.Shoot.performed += ctx => gun.Shoot();
        shooting.Reload.performed += ctx => specifcWeapon.Reload();
        shooting.Shoot.performed += ctx => specifcWeapon.Shoot();

        shooting.EquipPrimary.performed += ctx => gun.EquipPrimary();
        shooting.EquipPrimary.performed += ctx => gun.EquipSecondary();
        shooting.EquipPrimary.performed += ctx => gun.EquipKnife();
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
        //fireCoroutine = StartCoroutine(gun.RapidFire());
        fireCoroutine = StartCoroutine(specifcWeapon.RapidFire());
    }

    void StopFiring()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }
}