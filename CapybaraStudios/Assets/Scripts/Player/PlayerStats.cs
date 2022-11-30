using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth = 100;

    public TextMeshPro damageText;


    void Start()
    {
    }

    void Update()
    {
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount < 0) return;
        //take damage
        playerHealth -= damageAmount;
        damageText.text = damageAmount.ToString();
        //die if health is < 0
        if (playerHealth <= 0)
        {
            //die and respawn
        }
    }
}