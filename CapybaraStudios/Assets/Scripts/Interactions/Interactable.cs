using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //template methode pattern

    //tooltip message
    public string message;

    public void BaseInteract(GameObject player)
    {
        Interact(player);
    }

    protected virtual void Interact(GameObject player)
    {
        //template function
    }
}