using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public delegate void AnswerCallback(Vector3 origin, Vector3 hit, Transform player);

    public static event AnswerCallback doShoot;

    void Start()
    {
        doShoot += Shoot;
    }

    public void Shoot(Vector3 origin, Vector3 hit, Transform player)
    {
        Debug.Log(player.name + "shot");
    }

    public static void Shot(Vector3 origin, Vector3 hit, Transform player)
    {
        if (doShoot != null)
            doShoot(origin, hit, player);
    }
}