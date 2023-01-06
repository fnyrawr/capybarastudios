using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tutorialButton : Interactable
{
    public GameObject button;
    public TMP_Text text;
    private string[] messages = {"This is a Tutorial", "You can walk with WASD", "You can jump with SPACE", "You can sprint with SHIFT", "You can crouch with STRG", "You can slide with SHIFT + STRG", "You can pick items up with E ->", "You can scroll through your inventory with 123", "You can drop items with Q", "You Can Attack with Left MB (& Shoot)", "You can aim with Right MB (& Grapple)", "You can reload with R","Turn around and test some other weapons", "Then pass the door infront of you!", "Back to earth!"};
    private int counter = -1;
    public AudioSource keyboardSound;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Press E on the keyboard";
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override void Interact(GameObject player)
    {
        counter = counter + 1;
        Debug.Log(player.name + "interacted with " + gameObject.name);
        if(counter < messages.Length) {
            text.text = messages[counter];
        }
        keyboardSound.Play();
    }
}