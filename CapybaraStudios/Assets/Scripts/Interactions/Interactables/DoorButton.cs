using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : Interactable
{
    public GameObject button;

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
        Debug.Log(player.name+ "interacted with " + gameObject.name);
        var buttonRenderer = button.GetComponent<Renderer>();
        if(buttonRenderer.material.color.Equals(Color.red)) buttonRenderer.material.SetColor("_Color", Color.green);
        else buttonRenderer.material.SetColor("_Color", Color.red);
    }
}
