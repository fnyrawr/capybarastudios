using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpInput { get; private set; } = false;
    public bool SprintInput { get; private set; } = false;
    public bool CrouchInput { get; private set; } = false;

    private PlayerInput playerInput;
    public PlayerInput.WalkingActions walking;
    public PlayerInput.ShootingActions shooting;
    public PlayerInput.UIActions ui;
    private PlayerMovement movement;
    private GunScript gun;
    private HUDcontroller hud;


    void Awake()
    {
        playerInput = new PlayerInput();
        var rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
        {
            playerInput.LoadBindingOverridesFromJson(rebinds);
        }

        walking = playerInput.Walking;
        shooting = playerInput.Shooting;
        ui = playerInput.UI;

        gun = GetComponent<GunScript>();
        hud = GetComponentInChildren<HUDcontroller>();

        shooting.Special.started += ctx => gun.StartSpecial();
        shooting.Special.canceled += ctx => gun.StopSpecial();

        shooting.Shoot.started += ctx => gun.StartFiring();
        shooting.Shoot.performed += ctx => gun.Shoot();
        shooting.Shoot.canceled += ctx => gun.StopFiring();

        shooting.Reload.performed += ctx => gun.Reload();

        shooting.EquipPrimary.performed += ctx => equip(0);
        shooting.EquipSecondary.performed += ctx => equip(1);
        shooting.EquipKnife.performed += ctx => equip(2);
        shooting.EquipUtility.performed += ctx => equip(3);

        shooting.Drop.performed += ctx => gun.EjectGun();

        ui.Tab.started += ctx => hud.Tab();
        ui.Tab.canceled += ctx => hud.Tab();
        //ui.Pause.performed += ctx => hud.DoPause();
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
        walking.Crouch.canceled += SetCrouch;

        walking.Jump.started += SetJump;
        walking.Jump.canceled += SetJump;

        walking.Enable();
        shooting.Enable();
        ui.Enable();
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
        walking.Crouch.canceled -= SetCrouch;

        walking.Jump.started -= SetJump;
        walking.Jump.canceled -= SetJump;

        walking.Disable();
        shooting.Disable();
        ui.Disable();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }

    private void SetJump(InputAction.CallbackContext ctx)
    {
        JumpInput = ctx.started;
    }

    private void SetCrouch(InputAction.CallbackContext ctx)
    {
        CrouchInput = ctx.started;
    }

    private void SetSprint(InputAction.CallbackContext ctx)
    {
        SprintInput = ctx.started;
    }

    private void equip(int index)
    {
        gun.EquipWeapon(index);
    }
}