using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public Transform respawnPoint;
    private Vector3 respawnPosition;
    public GameObject player;
    public event CharacterSpawnedDelegate OnCharacterSpawned;
    public static int kills = 0;
    public static int damageDone = 0;
    public static string time = TimeSpan.Zero.ToString(@"hh\:mm\:ss");

    public delegate void CharacterSpawnedDelegate(GameObject player);

    private GameObject currentPlayer;

    private static Transform _dummy;
    public Transform dummy;

    private void Start()
    {
        _dummy = dummy;

        respawnPosition = respawnPoint.position;
        kills = 0;
        damageDone = 0;
        time = TimeSpan.Zero.ToString(@"hh\:mm\:ss");
        if (!GameObject.Find("Player"))
        {
            SpawnPlayer();
        }
        else
        {
            currentPlayer = GameObject.Find("Player");
        }
    }

    public void Update()
    {
        time = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString(@"hh\:mm\:ss");
    }

    public void Respawn()
    {
        Destroy(currentPlayer);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        currentPlayer = Instantiate(player, respawnPosition, Quaternion.identity);
        OnCharacterSpawned?.Invoke(currentPlayer);
    }

    public void changeRespawn(Transform t)
    {
        changeRespawn(t.position);
    }

    public void changeRespawn(Vector3 p)
    {
        respawnPosition = p;
    }


    public static void triggerRespawn(Vector3 pos)
    {
        Instantiate(_dummy, pos, Quaternion.Euler(0, 90, 0));
    }
}