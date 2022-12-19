using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class chest : Interactable
{
    private Animator chest_anim;
    private PlayableDirector chest_director;
    public GameObject player;
    public GameObject the_chest;

    void Start() {
        chest_anim = (Animator) the_chest.GetComponent("Animator");
        chest_director = (PlayableDirector) the_chest.GetComponent<PlayableDirector>();
    }

    protected override void Interact(GameObject player)
    {
        Debug.Log(player.name + "interacted with " + gameObject.name);
        chest_anim.enabled = true;
        chest_director.enabled = true;
    }
}
