using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //template methode pattern

    //tooltip message
    public string message;

    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    { 
        //template function
    }
}
