using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] InputManager _input;

    //sounds
    public AudioSource walkingSound;
    public AudioSource slidingSound;

    //movement
    private float playerVelocity;

    //crouching
    private bool crouching = false;

    //sprinting
    private bool sprinting = false;

    //sliding
    private float crouchingMomentum = 1f;

    //speed and jump
    public float speed = 7f;
    public float sprintingSpeed = 15f;
    public float jumpHeight = 2f;
    public float gravity = -40f;

    //hook
    [NonSerialized] public bool hooked;
    [NonSerialized] public Vector3 midAirMomentum;

    public Transform Target;

    private Animator _animator;
    private int _isCrouchingHash;
    private int _isFallingHash;
    private int _isSlidingHash;
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
        _isSlidingHash = Animator.StringToHash("isSliding");
    }

    // Update is called once per frame
    void Update()
    {
        if (hooked)
        {
            playerVelocity = -2f;
            if(_input.JumpInput) {
                GetComponentInChildren<GrapplingGun>().StopHook();
                playerVelocity = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
        }

        ProcessMovement(_input.MoveInput);
        Crouch();
        Sprint();
        Jump();
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
            }
            else if (controller.velocity.y < 0 || controller.velocity.y < -2)
            {
                isJumping = false;
                _animator.SetTrigger("isFalling");
            }

            _animator.SetBool("isJumping", isJumping);
        }

        _animator.SetBool("isGrounded", controller.isGrounded);
    }

    public void ProcessMovement(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        //translate vertical mocement to forwards/backwards movement
        moveDirection.z = input.y;
        //if player walks backwards speed can only be speed, else also sprintingSpeed
        var actualSpeed = input.y < 0 ? speed : (sprinting && crouchingMomentum >= 1f ? sprintingSpeed : speed);
        if(!walkingSound.isPlaying && input.y != 0) walkingSound.Play();
        controller.Move(crouchingMomentum * actualSpeed * Time.deltaTime * transform.TransformDirection(moveDirection));
        //decrease slidingMomentum
        if (crouchingMomentum > 0.65f && crouching)
        {
            float slideTime = 0.2f;
            crouchingMomentum -= Time.deltaTime * slideTime;
            if (crouchingMomentum < 0.65f)
            {
                crouchingMomentum = 0.65f;
                _animator.SetBool(_isCrouchingHash, crouching);
                _animator.SetBool(_isSlidingHash, false);
            }
        }

        //constant downward (gravity)
        playerVelocity += (gravity * Time.deltaTime);
        if (controller.isGrounded && playerVelocity < 0)
        {
            playerVelocity = -2f;
            midAirMomentum = Vector3.zero;
        }

        midAirMomentum.y = playerVelocity;
        controller.Move(midAirMomentum * Time.deltaTime);

        //decrease midAirMomentum 
        if (midAirMomentum.magnitude > 0f)
        {
            float drag = 3f; //how much the char gets dragged
            midAirMomentum -= midAirMomentum * drag * Time.deltaTime;
            if (midAirMomentum.magnitude < 0f) midAirMomentum = Vector3.zero;
        }

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
        if (controller.isGrounded && _input.JumpInput)
        {
            playerVelocity = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        if (crouching == _input.CrouchInput) return;
        if (!crouching && _input.CrouchInput)
        {
            if (sprinting)
            {
                crouchingMomentum = 1.1f;
                _animator.SetBool(_isSlidingHash, _input.CrouchInput);
                if(!slidingSound.isPlaying) {
                slidingSound.Play();
            }
            }
            else
            {
                crouchingMomentum = 0.65f;
                _animator.SetBool(_isCrouchingHash, _input.CrouchInput);
            }
        }

        if (crouching && !_input.CrouchInput)
        {
            crouchingMomentum = 1f;
            _animator.SetBool(_isCrouchingHash, _input.CrouchInput);
            _animator.SetBool(_isSlidingHash, _input.CrouchInput);
        }

        crouching = _input.CrouchInput;
    }

    public void Sprint()
    {
        if (crouching) return;
        sprinting = _input.SprintInput;
    }
}