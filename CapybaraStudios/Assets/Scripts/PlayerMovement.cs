using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    public float jumpHeight = 2f;
    public float gravity = -40f;


    private Animator _animator;
    private int isWalkingHash;
    private int isCrouchingHash;
    private int isSprintingHash;
    private int isFallingHash;
    private int sidewaysHash;
    private int forwardBackwardHash;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isCrouchingHash = Animator.StringToHash("isCrouched");
        isSprintingHash = Animator.StringToHash("isSprinting");
        isFallingHash = Animator.StringToHash("isFalling");
        sidewaysHash = Animator.StringToHash("movementSideways");
        forwardBackwardHash = Animator.StringToHash("movementForwards");
    }

    // Update is called once per frame
    void Update()
    {
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

        _animator.SetBool(isFallingHash, !isGrounded);
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        //translate vertical mocement to forwards/backwards movement
        moveDirection.z = input.y;

        //_animator.SetBool(isWalkingHash, input.y != 0);
        _animator.SetBool(isWalkingHash, input != Vector2.zero);
        _animator.SetFloat(sidewaysHash, input.x);
        _animator.SetFloat(forwardBackwardHash, input.y);


        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        //constant downward (gravity)
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.deltaTime);
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
        _animator.SetBool(isCrouchingHash, crouching);
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        _animator.SetBool(isSprintingHash, sprinting);
        if (sprinting)
            speed = 15f;
        else
            speed = 6f;
    }
}