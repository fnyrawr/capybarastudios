using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botRespawn : Interactable
{
    public GameObject button;
    public GameObject bot;
    public GameObject weapon1;
    private GameObject spawnedWeapon1;
    private GameObject spawnedBot;
    public GameObject[] respawns;
    public GameObject[] weapons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject player)
    {
        respawns = GameObject.FindGameObjectsWithTag("Respawn");
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        
        foreach (GameObject respawn in respawns)
        {
            spawnedBot = Instantiate(bot, respawn.transform.position, respawn.transform.rotation);
            spawnedBot.tag = "Respawn";
            Destroy(respawn);
            
        }
        foreach (GameObject weapon in weapons)
        {
            spawnedWeapon1 = Instantiate(weapon1, weapon.transform.position, weapon.transform.rotation);
            spawnedWeapon1.tag = "Weapon";
            Destroy(weapon);
            
        }
        Debug.Log(player.name + "interacted with " + gameObject.name);
    }
}
