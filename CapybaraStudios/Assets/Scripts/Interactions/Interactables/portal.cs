using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    private GameObject thePlayer;
    public Transform teleportTarget;
    public AudioSource portalSound;
    public AudioSource themeOne;
    public AudioSource themeTwo;
    public AudioSource themeOneIntense;
    public AudioSource themeTwoIntense;
    private PlayerStats playerStats;
    public bool changeMusic;

    void OnTriggerEnter(Collider other)
    {
        thePlayer = other.gameObject.transform.root.gameObject;
        thePlayer.transform.position = teleportTarget.transform.position;
        portalSound.Play();
        if (changeMusic)
        {
            themeOne.Stop();
            themeTwo.Play();
        }
    }

    void Start()
    {
        themeOne.Play();
    }

    void Update()
    {
        playerStats = thePlayer.GetComponent<PlayerStats>();
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
}