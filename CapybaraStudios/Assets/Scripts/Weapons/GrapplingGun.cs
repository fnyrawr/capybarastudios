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
    [SerializeField] private int hookSpeed = 30;
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
    }

    private void HandleHookMovement() {
        Vector3 dir = (hookPos - transform.parent.transform.position).normalized;
        playerController.Move(dir * Time.deltaTime * hookSpeed);
    }
}
