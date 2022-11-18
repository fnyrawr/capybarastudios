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
    private bool sprinting = false;
    [SerializeField]
    private float baseFOV;
    [SerializeField]
    private float sprintingFOV = 1.1f;
    private float elapsedTime = 0f;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate() {
        elapsedTime += Time.deltaTime;
        if(elapsedTime <= 1) {
            float percentage = elapsedTime / 0.5f;
            if(sprinting) camera.fieldOfView = Mathf.Lerp(baseFOV, baseFOV * sprintingFOV, Mathf.SmoothStep(0,1, percentage));
            else camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseFOV, Mathf.SmoothStep(0,1, percentage));
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
        Debug.Log("sprint");
        sprinting = !sprinting;
        elapsedTime = 0;
    }
}