using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botRespawn : MonoBehaviour
{
    public GameObject button;
    public GameObject bot1;
    public GameObject bot2;
    public GameObject bot3;
    public GameObject bot4;
    public GameObject bot5;
    public GameObject bot6;
    List<GameObject> generatedObjects = new List<GameObject>();

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
        Debug.Log(player.name + "interacted with " + gameObject.name);
        Destroy(bot1);
        Destroy(bot2);
        Destroy(bot3);
        Destroy(bot4);
        Destroy(bot5);
        Destroy(bot6);
    }
}
