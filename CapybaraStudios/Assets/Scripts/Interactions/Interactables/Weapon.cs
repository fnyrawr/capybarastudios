using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable
{
    public int weaponSlot;
    public int weaponType;
    

    /*
    weaponSlot
    
    primary = 1
    secondary = 2
    knife = 3
    utility = 4

    weaponType

    assault rifle = 1
    shotgun = 2
    submachine gun = 3
    machine gun = 4

    */

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