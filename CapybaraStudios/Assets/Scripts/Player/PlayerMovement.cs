using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Camera camera;
    //movement
    private float playerVelocity;
    //crouching
    private bool crouching = false;

    //sprinting
    private bool sprinting = false;

    //movement
    public float speed = 7f;
    public float sprintingSpeed = 15f;
    public float jumpHeight = 2f;
    public float gravity = -40f;
    //hook
    [NonSerialized] public bool hooked;
    
    
    public Transform Target;

    private Animator _animator;
    private int _isCrouchingHash;
    private int _isFallingHash;
    private int _sidewaysHash;
    private int _forwardBackwardHash;
    public float _animationTransitionSpeed = 3.0f;
    private float _velocityX = 0;
    private float _velocityZ = 0;
    private bool isJumping;

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

    // Update is called once per frame
    void Update()
    {
            //constant downward (gravity)
        playerVelocity += (gravity * Time.deltaTime);
        if (controller.isGrounded && playerVelocity < 0)
        {
            playerVelocity = -2f;
        }
        controller.Move(new Vector3(0, playerVelocity * Time.deltaTime,0));

        if (controller.isGrounded)
        {
            _animator.SetBool("isFalling", false);
        }
        else
        {
            isJumping = false;
            if (controller.isGrounded) //if the player was grounded in the previous update but nor now, meaning he jumped now
            {
                isJumping = true;
                _animator.SetBool("isFalling", false);
            }
            else if ((isJumping && controller.velocity.y < 0) || controller.velocity.y < -2)
            {
                isJumping = false;
                _animator.SetBool("isFalling", true);
            }

            _animator.SetBool("isJumping", isJumping);
        }
        _animator.SetBool("isGrounded", controller.isGrounded);
    }

    public void ProcessMove(Vector2 input)
    {
        if(hooked) return;
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        //translate vertical mocement to forwards/backwards movement
        moveDirection.z = input.y;
        //if player walks backwards speed can only be speed, else also sprintingSpeed
        var actualSpeed = input.y < 0 ? speed : sprinting ? sprintingSpeed : speed;
        controller.Move(actualSpeed * Time.deltaTime * transform.TransformDirection(moveDirection));
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
        if (controller.isGrounded)
        {
            playerVelocity = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        if(sprinting) return;
        crouching = !crouching;
        _animator.SetBool(_isCrouchingHash, crouching);
    }

    public void Sprint()
    {
        if(crouching) return;
        sprinting = !sprinting;
    }
}