using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3.0f;
    [SerializeField] private bool showRay = true;
    [SerializeField] private LayerMask mask;

    private PlayerUI playerUI;
    private InputManager inputManager;

    void Start()
    {
        cam = GetComponent<PlayerLook>().camera;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);

        //shoot frontal ray at the center of the screen
        Ray ray = new(cam.transform.position, cam.transform.forward);
        if (showRay) Debug.DrawRay(ray.origin, ray.direction * distance);
        //hit?
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, distance, mask)) return;
        //is interactable?
        if (hitInfo.collider.GetComponent<Interactable>() == null) return;
        //get interactable as variable
        Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
        //update tooltip text
        playerUI.UpdateText(interactable.message);
        //on E press
        if (inputManager.walking.Interact.triggered)
        {
            print(interactable.name);
            interactable.BaseInteract(gameObject);
        }
    }
}