using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    public GameObject thePlayer;
    public Transform respawnTarget;
    public float heightOffset;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(thePlayer.transform.position.y < heightOffset) {
            thePlayer.transform.position = respawnTarget.transform.position;
            playerStats = thePlayer.GetComponent<PlayerStats>();
            playerStats.TakeDamage(playerStats.maxHealth);
            playerStats.currentHealth = playerStats.maxHealth;
            playerStats.Heal(playerStats.maxHealth);
            playerStats.UpdateHealth();
            playerStats.updateVignette();
            // thePlayer.GetComponent<Ragdoll>().Awake();
        }
        
    }
}
