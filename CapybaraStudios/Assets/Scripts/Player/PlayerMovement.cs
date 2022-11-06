using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class PlayerMovement : NetworkBehaviour
{
    private CharacterController controller;
    private bool isGrounded;
    //crouching
    private bool crouching = false;
    private bool lerpCrouch;

    private float crouchTimer = 0;

    //movement
    public float speed = 7f;
    //sprinting
    private bool sprinting = false;
    public float sprintingSpeed = 15f;
    public float jumpHeight = 2f;
    public float gravity = -40f;


    private Animator _animator;
    private int _isCrouchingHash;
    private int _isFallingHash;
    private int _sidewaysHash;
    private int _forwardBackwardHash;
    public float _animationTransitionSpeed = 3.0f;

    //Networking 
    [SerializeField]
    private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>();
    [SerializeField]
    private NetworkVariable<Vector3> networkVelocity = new NetworkVariable<Vector3>();
    //[SerializeField]
    //private NetworkVariable<PlayerState> networkState = new NetworkVariable<PlayerState>();

    private Vector3 oldInputPosition;
    private float oldVelocity;
    private Vector3 oldAnimationVelocity;
    //private PlayerState playerState;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _isCrouchingHash = Animator.StringToHash("isCrouched");
        _isFallingHash = Animator.StringToHash("isFalling");
        _sidewaysHash = Animator.StringToHash("movementSideways");
        _forwardBackwardHash = Animator.StringToHash("movementForwards");
    }
    void Update()
    {   
        if(IsClient && IsOwner) isGrounded = controller.isGrounded;
        ClientMove();
        ClientVisuals();
        /*if (lerpCrouch)
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

        _animator.SetBool(_isFallingHash, !isGrounded);*/

    }

    private void ClientMove() {
        if(networkPosition.Value != Vector3.zero) {
            controller.Move(networkPosition.Value * Time.fixedDeltaTime);
        }
    }

    private void ClientVisuals() {
        if(networkVelocity.Value != Vector3.zero) {
            _animator.SetFloat(_sidewaysHash, networkVelocity.Value.x);
            _animator.SetFloat(_forwardBackwardHash, networkVelocity.Value.z);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(Vector3 pos) {
        networkPosition.Value = pos;
    }
    [ServerRpc]
    public void UpdateAnimationServerRpc(Vector3 vel) {
        networkVelocity.Value = vel;
    }
    // Update is called once per frame

    public void ProcessMove(Vector2 input)
    {
        if(!(IsClient && IsOwner)) return; 
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        //translate vertical mocement to forwards/backwards movement
        moveDirection.z = input.y;
        //if player walks backwards speed can only be speed, else also sprintingSpeed
        var actualSpeed = input.y < 0 ? speed : sprinting ? sprintingSpeed : speed;
        Vector3 inputPosition = actualSpeed * transform.TransformDirection(moveDirection);

        //constant downward (gravity)
        oldVelocity += gravity  * Time.deltaTime;
        if (isGrounded && oldVelocity< 0)
        {
            oldVelocity = -2f;
        }
        // update on Server
        inputPosition = new Vector3(inputPosition.x, oldVelocity, inputPosition.z);
        if(oldInputPosition != inputPosition) {
            oldInputPosition = inputPosition;
            UpdateClientPositionServerRpc(inputPosition);
        }


        //general animation controlling
        Vector3 animationVelocity = oldAnimationVelocity;
        //das kann in eine Funktion
        if (input.x > 0 && animationVelocity.x < input.x || input.x < 0 && animationVelocity.x > input.x)
        {
            animationVelocity.x += input.x * Time.deltaTime * _animationTransitionSpeed;
        }
        else if (input.x == 0 && animationVelocity.x != 0)
        {
            if (Math.Abs(animationVelocity.x ) < 0.1)
            {
                //resetting velocity if its really small to prevent
                //it from gittering the character around
                animationVelocity.x  = 0;
            }
            else animationVelocity.x += (animationVelocity.x < 0 ? 1 : -1) * Time.deltaTime * _animationTransitionSpeed;
        }

        if (input.y > 0 && animationVelocity.z < input.y || input.y < 0 && animationVelocity.z > input.y)
        {
            animationVelocity.z+= input.y * Time.deltaTime * _animationTransitionSpeed;
        }
        else if (input.y == 0 && animationVelocity.z != 0)
        {
            if (Math.Abs(animationVelocity.z) < 0.1)
            {
                //resetting velocity if its really small to prevent
                //it from gittering the character around
                animationVelocity.z = 0; 
            }
            else animationVelocity.z += (animationVelocity.z < 0 ? 1 : -1) * Time.deltaTime * _animationTransitionSpeed;
        }
        if(animationVelocity != oldAnimationVelocity) {
            oldAnimationVelocity = animationVelocity;
            UpdateAnimationServerRpc(animationVelocity * (sprinting ? 4 : 1));
        }
    }

    public void Jump()
    {
        if(!(IsClient && IsOwner)) return; 
        if (isGrounded)
        {
            oldVelocity = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            UpdateClientPositionServerRpc(new Vector3(oldInputPosition.x, oldVelocity, oldInputPosition.z));
        }
    }

    public void Crouch()
    {
        if(!(IsClient && IsOwner)) return; 
        /*crouching = !crouching;
        _animator.SetBool(_isCrouchingHash, crouching);
        crouchTimer = 0;*/
        //lerpCrouch = true;
    }

    public void Sprint()
    {
        if(!(IsClient && IsOwner)) return; 
        sprinting = !sprinting;
    }
}