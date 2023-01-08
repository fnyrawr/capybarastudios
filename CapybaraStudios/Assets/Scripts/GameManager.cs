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

    public AudioSource themeOne;
    public AudioSource themeTwo;
    public AudioSource themeOneIntense;
    public AudioSource themeTwoIntense;
    private PlayerStats playerStats;


    private void Start()
    {
        themeOne.Play();

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


        playerStats = player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth < playerStats.maxHealth)
        {
            if (themeOne.isPlaying)
            {
                themeOneIntense.Play();
                if (themeOneIntense.isPlaying)
                {
                    themeOne.Stop();
                }
            }

            if (themeTwo.isPlaying)
            {
                themeTwoIntense.Play();
                if (themeTwoIntense.isPlaying)
                {
                    themeTwo.Stop();
                }
            }
        }
        else
        {
            if (themeOneIntense.isPlaying)
            {
                themeOne.Play();
                themeOneIntense.Stop();
            }

            if (themeTwoIntense.isPlaying)
            {
                themeTwo.Play();
                themeTwoIntense.Stop();
            }
        }
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

    public void teleport()
    {
        themeOne.Stop();
        themeTwo.Play();
    }
}