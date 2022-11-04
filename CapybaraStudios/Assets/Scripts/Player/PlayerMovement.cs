using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
public class PlayerMovement : NetworkBehaviour
{
    private CharacterController controller;

    //movement
    private Vector3 playerVelocity;

    private bool isGrounded;

    //crouching
    private bool crouching = false;
    private bool lerpCrouch;

    private float crouchTimer = 0;

    //sprinting
    private bool sprinting = false;

    //movement
    public float speed = 7f;
    public float sprintingSpeed = 15f;
    public float jumpHeight = 2f;
    public float gravity = -40f;


    private Animator _animator;
    private int _isCrouchingHash;
    private int _isFallingHash;
    private int _sidewaysHash;
    private int _forwardBackwardHash;
    public float _animationTransitionSpeed = 3.0f;
    private float _velocityX = 0;
    private float _velocityZ = 0;

    //Networking 

    private NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();

    private Transform old;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _isCrouchingHash = Animator.StringToHash("isCrouched");
        _isFallingHash = Animator.StringToHash("isFalling");
        _sidewaysHash = Animator.StringToHash("movementSideways");
        _forwardBackwardHash = Animator.StringToHash("movementForwards");
    }

    private void UpdateServer() {
        transform.position = position.Value;
    }

    private void UpdateClient() {
        Transform t = null;

        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        _animator.SetBool(_isFallingHash, !isGrounded);

        if(!transform.Equals(old)) {
            old = transform;
            UpdateClientPositionServerRpc(transform.position);
        }
    }
    [ServerRpc]
    public void UpdateClientPositionServerRpc(Vector3 vec) {
        position.Value = vec;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsServer) {
            UpdateServer();
        }

        if(IsClient && IsOwner) {
            UpdateClient();
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        //translate vertical mocement to forwards/backwards movement
        moveDirection.z = input.y;
        //if player walks backwards speed can only be speed, else also sprintingSpeed
        var actualSpeed = input.y < 0 ? speed : sprinting ? sprintingSpeed : speed;
        controller.Move(actualSpeed * Time.deltaTime *
                        transform.TransformDirection(moveDirection));

        //constant downward (gravity)
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.deltaTime);


        //general animation controlling
        var s = sprinting ? 4 : 1;
        if (input.x > 0 && _velocityX < input.x || input.x < 0 && _velocityX > input.x)
        {
            _velocityX += input.x * Time.deltaTime * _animationTransitionSpeed;
        }
        else if (input.x == 0 && _velocityX != 0)
        {
            if (Math.Abs(_velocityX) < 0.1)
            {
                _velocityX =
                    0; //resetting velocity if its really small to prevent it from gittering the character around
            }
            else _velocityX += (_velocityX < 0 ? 1 : -1) * Time.deltaTime * _animationTransitionSpeed;
        }

        if (input.y > 0 && _velocityZ < input.y || input.y < 0 && _velocityZ > input.y)
        {
            _velocityZ += input.y * Time.deltaTime * _animationTransitionSpeed;
        }
        else if (input.y == 0 && _velocityZ != 0)
        {
            if (Math.Abs(_velocityZ) < 0.1)
            {
                _velocityZ =
                    0; //resetting velocity if its really small to prevent it from gittering the character around
            }
            else _velocityZ += (_velocityZ < 0 ? 1 : -1) * Time.deltaTime * _animationTransitionSpeed;
        }

        _animator.SetFloat(_sidewaysHash, _velocityX * s);
        _animator.SetFloat(_forwardBackwardHash, _velocityZ * s);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        _animator.SetBool(_isCrouchingHash, crouching);
        crouchTimer = 0;
        //lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
    }
}