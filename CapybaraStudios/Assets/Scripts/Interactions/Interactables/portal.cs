using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    private GameObject thePlayer;
    public Transform teleportTarget;
    public AudioSource portalSound;
    public bool changeMusic;


    void OnTriggerEnter(Collider other)
    {
        thePlayer = other.gameObject.transform.root.gameObject;
        portalSound.Play();
        if (changeMusic)
        {
            FindObjectOfType<GameManager>().teleport();
        }
        thePlayer.transform.position = teleportTarget.transform.position;
    }
}