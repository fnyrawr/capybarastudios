using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //template methode pattern

    //tooltip message
    public string message;
    public float cooldownTime;

    bool ready = true;

    private void Awake() {
        ready = true;
    }

    public void BaseInteract(GameObject player)
    {
        if(ready)
        {
            Interact(player);
            Invoke("Cooldown", cooldownTime);
            ready = false;
        } else {
            Debug.Log("Not ready yet");
            Invoke("Reset", 3);
        }
        
    }

    protected virtual void Interact(GameObject player)
    {
        //template function
    }

    private void Cooldown()
    {
        ready = true;
    }
    private void Reset()
    {
        ready = true;
    }
}