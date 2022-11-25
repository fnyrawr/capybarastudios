using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public Vector2 MoveInput {get; private set; }
    public Vector2 LookInput {get; private set; }
    public bool JumpInput {get; private set; } = false;
    public bool SprintInput {get; private set; } = false;
    public bool CrouchInput {get; private set; } = false;

    private PlayerInput playerInput;
    public PlayerInput.WalkingActions walking;
    public PlayerInput.ShootingActions shooting;

    private PlayerMovement movement;
    private GunScript gun;
    private GrapplingGun hook;
    private SpecificWeaponScript specifcWeapon;

    Coroutine fireCoroutine;

    void Awake()
    {
        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        shooting = playerInput.Shooting;
        gun = GetComponent<GunScript>();
        hook = GetComponentInChildren<GrapplingGun>();
        if (GetComponentInChildren<SpecificWeaponScript>() != null)
        {
            specifcWeapon = GetComponentInChildren<SpecificWeaponScript>();
            Debug.Log("SpecificWeaponScript gefunden");
        }
            

        walking.Grappling.started += ctx => hook.Hook();
        walking.Grappling.canceled += ctx => hook.StopHook();

        shooting.Shoot.started += ctx => StartFiring();
        shooting.Shoot.canceled += ctx => StopFiring();

        shooting.Reload.performed += ctx => specifcWeapon.Reload();
        shooting.Shoot.performed += ctx => specifcWeapon.Shoot();

        shooting.EquipPrimary.performed += ctx => gun.EquipPrimary();
        shooting.EquipPrimary.performed += ctx => gun.EquipSecondary();
        shooting.EquipPrimary.performed += ctx => gun.EquipKnife();
    }

    private void OnEnable()
    {
        walking.Movement.performed += SetMove;
        walking.Movement.canceled += SetMove;

        walking.LookAround.performed += SetLook;
        walking.LookAround.canceled += SetLook;

        walking.Sprint.started += SetSprint;
        walking.Sprint.canceled += SetSprint;
        
        walking.Crouch.started += SetCrouch;

        walking.Jump.started += SetJump;
        walking.Jump.canceled += SetJump;

        walking.Enable();
        shooting.Enable();
    }

    private void OnDisable()
    {
        walking.Movement.performed -= SetMove;
        walking.Movement.canceled -= SetMove;

        walking.LookAround.performed -= SetLook;
        walking.LookAround.canceled -= SetLook;

        walking.Sprint.started -= SetSprint;
        walking.Sprint.canceled -= SetSprint;
        
        walking.Crouch.started -= SetCrouch;

        walking.Jump.started -= SetJump;
        walking.Jump.canceled -= SetJump;

        walking.Disable();
        shooting.Disable();
    }

    private void SetMove(InputAction.CallbackContext ctx) {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void SetLook(InputAction.CallbackContext ctx) {
        LookInput = ctx.ReadValue<Vector2>();
    }

    private void SetJump(InputAction.CallbackContext ctx) {
        JumpInput = ctx.started;
    }

    private void SetCrouch(InputAction.CallbackContext ctx) {
        CrouchInput = !CrouchInput;
    }

    private void SetSprint(InputAction.CallbackContext ctx) {
        SprintInput = ctx.started;
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