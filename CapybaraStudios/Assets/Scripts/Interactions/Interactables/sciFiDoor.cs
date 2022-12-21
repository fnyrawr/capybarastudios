using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sciFiDoor : Interactable
{
    public GameObject button;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public AudioSource doorSound;

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
        doorSound.Play();
        Debug.Log(player.name + "interacted with " + gameObject.name);
        var buttonRenderer = button.GetComponent<Renderer>();
        if(buttonRenderer.material.color.Equals(Color.red)) {
            buttonRenderer.material.SetColor("_Color", Color.green);
            leftDoor.transform.localPosition = new Vector3(0, 0, 3);
            rightDoor.transform.localPosition = new Vector3(0, 0, -4);
        }
        else {
            buttonRenderer.material.SetColor("_Color", Color.red);
            leftDoor.transform.localPosition = new Vector3(0, 0, 0);
            rightDoor.transform.localPosition = new Vector3((float)0.007935028, (float)-0.1499473, (float)-1.433132);
        }
    }
}
