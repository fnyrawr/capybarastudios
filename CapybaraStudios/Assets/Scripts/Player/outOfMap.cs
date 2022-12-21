using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOfMap : MonoBehaviour
{
    public GameObject thePlayer;
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
            playerStats = thePlayer.GetComponent<PlayerStats>();
            playerStats.die();
        }
        
    }
}
