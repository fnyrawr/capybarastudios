using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable
{
    public int weaponSlot;
    public int animationType;
    
    void Start()
    {
        message = "Pick up [E]";
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override void Interact(GameObject player)
    {
        Debug.Log("Picked up " + gameObject.name);
        player.GetComponent<GunScript>().pickUp(gameObject);
    }
}