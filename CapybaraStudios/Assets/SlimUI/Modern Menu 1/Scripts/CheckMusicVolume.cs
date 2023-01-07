using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace SlimUI.ModernMenu
{
    public class CheckMusicVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        public void Start()
        {
            UpdateVolume();
        }

        public void UpdateVolume()
        {
            audioMixer.SetFloat("MusicVolume", 20f * Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")));
            audioMixer.SetFloat("SFXVolume", 20f * Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")));
        }
    }
}