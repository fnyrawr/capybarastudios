using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    Camera playerCam;
    CharacterController playerController;
    PlayerMovement playerMovement;
    public Transform debug;
    // Start is called before the first frame update
    private Vector3 hookPos;
    private bool hooked;
    private float hookSpeed = 0;
    private Vector3 hookDir;
    [SerializeField] float speedMin = 25f;
    [SerializeField] float speedMax = 60f;
    void Awake()
    {
        playerCam = transform.parent.GetComponentInChildren<Camera>();
        playerController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        //Debug.Log(playerCam.transform.position);
        //Debug.Log(playerController.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(!hooked) return;
        HandleHookMovement();
    }

    //call this function in inputscript to hook
    public void Hook() {
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit)) {
            debug.position = hit.point;
            hooked = true;
            hookPos = hit.point;
            playerMovement.hooked = true;
        }
    }

    public void StopHook() {
        hooked = false;
        playerMovement.hooked = false;
        playerMovement.midAirMomentum = hookSpeed * hookDir;
    }

    private void HandleHookMovement() {
        hookDir = (hookPos - transform.parent.transform.position).normalized;
        hookSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookPos), speedMin, speedMax);
        playerController.Move(hookDir * Time.deltaTime * hookSpeed);

        float closestDistance = 3f; //set to different value if player shouldnt perma grapple to wall
        if(Vector3.Distance(transform.position, hookPos) <= closestDistance) {
            hookSpeed = 0f;
        }
    }
}
