using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable
{
    public int weaponType;

    //rifile = 1
    //pistol = 2
    //knife = 3
    // Start is called before the first frame update
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