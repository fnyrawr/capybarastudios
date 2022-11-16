using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public GameObject thePlayer;
    public Transform teleportTarget;

    void OnTriggerEnter(Collider other) {
        thePlayer.transform.position = teleportTarget.transform.position;
    }
}
