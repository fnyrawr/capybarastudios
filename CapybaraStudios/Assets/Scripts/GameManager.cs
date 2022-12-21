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
    public delegate void CharacterSpawnedDelegate(GameObject player);
    public class OnCharacterSpawnedEventArgs : EventArgs {
        public GameObject player;
    }
    private GameObject currentPlayer;
    private void Start()
    {
        if (!GameObject.Find("Player")) {
            SpawnPlayer();
        } else {
            currentPlayer = GameObject.Find("Player");
        }
        
    }

    public void Respawn()
    {
        Destroy(GameObject.Find("Player"));
        SpawnPlayer();
    }

    public void SpawnPlayer() {
        currentPlayer = Instantiate(player, respawnPoint.position, Quaternion.identity);
        OnCharacterSpawned?.Invoke(currentPlayer);
    }
}