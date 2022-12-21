using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject player;

    public void Respawn()
    {
        Destroy(GameObject.Find("Player"));
        Instantiate(player, respawnPoint.position, Quaternion.identity);
    }
}