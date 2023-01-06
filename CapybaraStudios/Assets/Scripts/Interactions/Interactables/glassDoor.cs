using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glassDoor : Interactable
{
    public GameObject button;
    public GameObject door;
    public AudioSource buttonSound;

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject player)
    {
        Debug.Log(player.name + "interacted with " + gameObject.name);
        door.transform.localRotation = Quaternion.Euler(0, 90, 0);
        button.transform.localPosition = new Vector3(833, -78, 170);
        buttonSound.Play();
        
    }
}
