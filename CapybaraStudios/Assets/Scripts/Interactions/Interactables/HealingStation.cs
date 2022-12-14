using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HealingStation : Interactable
{
    private Material healingMaterial;
    private bool ready = true;
    public float cd = 2;
    public int healingPower = 20;

    void Awake()
    {
        healingMaterial = gameObject.GetComponent<MeshRenderer>().materials[3];
    }

    protected override void Interact(GameObject player)
    {
        if (ready)
        {
            print("TODO HEAL SOUND");
            var playerStats = player.GetComponent<PlayerStats>();
            playerStats.Heal(healingPower);
            message = "";
            healingMaterial.color = Color.red;
            ready = false;
            cd = 2;
        }
    }

    void FixedUpdate()
    {
        if (!ready)
        {
            cd -= Time.deltaTime;
            healingMaterial.color = Color.green * (1 - cd / 2) + Color.red * cd / 2;
            if (cd <= 0)
            {
                ready = true;
                cd = 2;
                message = "[E] Heal";
            }
        }
    }
}