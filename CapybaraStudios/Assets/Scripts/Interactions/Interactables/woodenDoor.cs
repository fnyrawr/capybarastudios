using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodenDoor : Interactable
{
    public GameObject button;
    public GameObject door;
    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject player)
    {
        Debug.Log(player.name + "interacted with " + gameObject.name);
        if(open == false) {
            door.transform.localRotation = Quaternion.Euler(0, 0, 0);
            button.transform.localPosition = new Vector3(833, -78, 170);
        }
        else {
            door.transform.localRotation = Quaternion.Euler(0, -90, 0);
            button.transform.localPosition = new Vector3(830, -78, 174);
        }
    }
}
