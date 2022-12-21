using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject player;
    public event CharacterSpawnedDelegate OnCharacterSpawned;
    public static int kills = 0;
    public static int damageDone = 0;

    public delegate void CharacterSpawnedDelegate(GameObject player);

    private GameObject currentPlayer;

    private void Start()
    {
        if (!GameObject.Find("Player"))
        {
            SpawnPlayer();
        }
        else
        {
            currentPlayer = GameObject.Find("Player");
        }
    }

    public void Respawn()
    {
        Destroy(currentPlayer);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        currentPlayer = Instantiate(player, respawnPoint.position, Quaternion.identity);
        OnCharacterSpawned?.Invoke(currentPlayer);
    }
}