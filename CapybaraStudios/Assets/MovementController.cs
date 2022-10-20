using UnityEngine;

public class MovementController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    private Transform _tr;
    private Vector3 _velocity;
    public float gravity = -19.62f;
    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;
    private bool _isGrounded;
    public float jumpHeight = 3f;


    private void Start()
    {
        _tr = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var move = _tr.right * x + _tr.forward * z;
        controller.Move(move * speed * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }
}