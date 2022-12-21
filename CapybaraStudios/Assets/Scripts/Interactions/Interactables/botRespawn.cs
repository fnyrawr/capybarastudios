using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botRespawn : Interactable
{
    public GameObject button;
    public GameObject bot;
    private GameObject spawnedBot;
    public GameObject[] respawns;

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
        
        foreach (GameObject respawn in respawns)
        {
            spawnedBot = Instantiate(bot, respawn.transform.position, respawn.transform.rotation);
            spawnedBot.tag = "Respawn";
            Destroy(respawn);
            
        }
        Debug.Log(player.name + "interacted with " + gameObject.name);
    }
}
