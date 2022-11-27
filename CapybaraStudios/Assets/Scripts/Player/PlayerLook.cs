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
    private bool sprinting = false, crouching = false;
    [SerializeField] private float baseFOV;
    [SerializeField] private float sprintingFOV = 1.1f;
    private float elapsedTime = 0f, elapsedcTime = 0f;
    private float height = 0f, currheight;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        height = camera.transform.position.y;
        currheight = height;
    }

    void FixedUpdate()
    {
        if (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / 0.5f;
            if (sprinting)
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV * sprintingFOV,
                    Mathf.SmoothStep(0, 1, percentage));
            else camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV, Mathf.SmoothStep(0, 1, percentage));
        }

        if (elapsedcTime <= 1)
        {
            elapsedcTime += Time.deltaTime;
            float percentage = elapsedTime / 0.3f;
            if (crouching) currheight = Mathf.Lerp(currheight, height - 0.5f, Mathf.SmoothStep(0, 1, percentage));
            else currheight = Mathf.Lerp(currheight, height, Mathf.SmoothStep(0, 1, percentage));
            camera.transform.localPosition = new Vector3(0, currheight, 0);
        }
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //calculate camera rotation for up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //apply to camera
        //camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        target.localRotation = Quaternion.Euler(xRotation, 0, 0);
        camera.transform.LookAt(targetPoint);
        //rotate Player for left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    public void Sprint()
    {
        if (crouching) return;
        sprinting = !sprinting;
        elapsedTime = 0;
    }

    public void Crouch()
    {
        if (sprinting) return;
        crouching = !crouching;
        elapsedcTime = 0;
    }
}