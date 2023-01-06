using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private float xRotation = 0f;
    public Camera camera;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    public Transform target;
    public Transform targetPoint;
    private bool sprinting = false, crouching = false, hooked = false;
    [SerializeField] private float baseFOV;
    [SerializeField] private float sprintingFOV = 1.1f;
    private float elapsedTime = 1f, elapsedcTime = 1f, elapsedzTime = 1f;
    private float height = 0f, currheight;
    [SerializeField] InputManager _input;
    [SerializeField] ParticleSystem hookParticles;
    private Quaternion hookRot;
    private float zoom = 0f;
    private void Start()
    {
        height = camera.transform.position.y;
        currheight = height;
    }
    
    void FixedUpdate()
    {
        //for zooming
        if (elapsedzTime <= 0.4f)
        {
            elapsedzTime += Time.deltaTime;
            float percentage = elapsedzTime / 0.4f;
            if (zoom > 0f) camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV * zoom, Mathf.SmoothStep(0, 1, percentage));
            else camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV, Mathf.SmoothStep(0, 1, percentage));
        }

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

        //for sneaking
        /*if (elapsedcTime <= 0.4f)
        {
            elapsedcTime += Time.deltaTime;
            float percentage = elapsedTime / 0.4f;
            if (crouching)
            {
                currheight = Mathf.Lerp(currheight, height - 0.5f, Mathf.SmoothStep(0, 1, percentage));
                if (sprinting)
                    camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV, Mathf.SmoothStep(0, 1, percentage));
            }
            else
            {
                currheight = Mathf.Lerp(currheight, height, Mathf.SmoothStep(0, 1, percentage));
                if (sprinting)
                    camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV * sprintingFOV,
                        Mathf.SmoothStep(0, 1, percentage));
            }

            //camera.transform.localPosition = new Vector3(0, currheight, 0);
        }*/
    }

    private void Update()
    {
        ProcessLook(_input.LookInput);
        Sprint();
        Crouch();
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

        if (hooked)
        {
            if (Quaternion.Angle(transform.localRotation, hookRot) > 50f) hookParticles.Stop();
            else if (hookParticles.isStopped) hookParticles.Play();
        }
    }

    public void StartHook()
    {
        hookParticles.Play();
        hooked = true;
        hookRot = transform.rotation;
        elapsedTime = 0;
    }

    public void StopHook()
    {
        hookParticles.Stop();
        hooked = false;
        elapsedTime = 0;
    }

    public void Sprint()
    {
        if (crouching || sprinting == _input.SprintInput || zoom > 0f) return;
        sprinting = _input.SprintInput;
        elapsedTime = 0;
    }

    public void Zoom(float zoom) {
        sprinting = false;
        this.zoom = zoom;
        elapsedzTime = 0f;
    }

    public void Crouch()
    {
        if (crouching == _input.CrouchInput) return;
        crouching = _input.CrouchInput;
        elapsedcTime = 0;
    }
}