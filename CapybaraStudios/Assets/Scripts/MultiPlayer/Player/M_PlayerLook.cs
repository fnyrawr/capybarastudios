using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class M_PlayerLook : NetworkBehaviour
{
    private float xRotation = 0f;
    [HideInInspector] public Camera camera;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    public Transform target;
    public Transform targetPoint;
    private bool sprinting = false, crouching = false, hooked = false;
    [SerializeField] private float baseFOV;
    [SerializeField] private float sprintingFOV = 1.1f;
    private float elapsedTime = 1f, elapsedcTime = 1f, elapsedzTime = 1f;
    [SerializeField] M_InputManager _input;
    private float zoom = 0f;
    [SerializeField] private Transform head;
    void Start() {
        if(!IsOwner) return;
        M_Camera.Instance.AttachToPlayer(head);
        camera = M_Camera.Instance._camera;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        if(!IsOwner) return;
        //for sprinting
        if (elapsedTime <= 0.4f)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / 0.4f;
            if (sprinting || hooked)
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV * sprintingFOV,
                    Mathf.SmoothStep(0, 1, percentage));
            else camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV, Mathf.SmoothStep(0, 1, percentage));
        }
    }

    private void Update()
    {
        if(!IsOwner) return;
        ProcessLook(_input.LookInput);
        Sprint();
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //calculate camera rotation for up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //rotate Player for left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        //apply to camera
        target.localRotation = Quaternion.Euler(xRotation, 0, 0);
        camera.transform.LookAt(targetPoint);
    }

    public void Sprint()
    {
        if (crouching || sprinting == _input.SprintInput || zoom > 0f) return;
        sprinting = _input.SprintInput;
        elapsedTime = 0;
    }
}
