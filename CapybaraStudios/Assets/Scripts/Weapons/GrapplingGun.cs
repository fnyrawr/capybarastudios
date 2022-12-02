using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    Camera playerCam;
    CharacterController playerController;
    PlayerMovement playerMovement;
    PlayerLook cameraScript;
    private Vector3 hookPos;
    private bool hooked, draw;
    private float hookSpeed = 0;
    private Vector3 hookDir;
    [SerializeField] float speedMin = 25f;
    [SerializeField] float speedMax = 60f;
    [SerializeField] LineRenderer lr;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private Transform gunTip;
    private float throwingTimer;
    [SerializeField] private float cooldown;
    private float currCooldown;
    void Awake()
    {
        playerCam = transform.parent.GetComponentInChildren<Camera>();
        cameraScript = transform.parent.GetComponent<PlayerLook>();
        playerController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        lr.positionCount = 0;
        currCooldown = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        currCooldown += Time.deltaTime;
        if(!hooked) return;
        HandleHookMovement();
    }

    private void LateUpdate() {
        if(!draw && hooked) {
            UpdateRope();
            return;
        } 
        else if(!draw) return;
        DrawRope();
    }

    private void DrawRope() {
        throwingTimer += Time.deltaTime;
        float delta = throwingTimer / 0.1f;
        Vector3 ropePos = Vector3.Lerp(gunTip.position, hookPos, delta);
        lr.SetPosition((int) 1, ropePos);
        if(delta >= 1) {
            hooked = true;
            draw = false;
            cameraScript.StartHook();
        }
    }

    private void UpdateRope() {
        lr.SetPosition((int) 0, gunTip.position);
    }

    //call this function in inputscript to hook
    public void Hook() {
        if(currCooldown >= cooldown && Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, maxDistance)) {
            draw = true;
            hookPos = hit.point;
            playerMovement.hooked = true;
            lr.positionCount = 2;
            lr.SetPosition((int) 0, gunTip.position);
        }
    }

    public void StopHook() {
        if(hooked) currCooldown = 0;
        draw = false;
        hooked = false;
        playerMovement.hooked = false;
        playerMovement.midAirMomentum = hookSpeed * hookDir;
        hookSpeed = 0;
        hookDir = Vector3.zero;
        lr.positionCount = 0;
        throwingTimer = 0;
        cameraScript.StopHook();
    }

    private void HandleHookMovement() {
        hookDir = (hookPos - transform.parent.transform.position).normalized;
        hookSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookPos), speedMin, speedMax);
        playerController.Move(hookDir * Time.deltaTime * hookSpeed);

        float closestDistance = 5f; //set to different value if player shouldnt perma grapple to wall
        if(Vector3.Distance(transform.position, hookPos) <= closestDistance) {
            hookSpeed = 0f;
        }
    }
}
