using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckMusicVolume : MonoBehaviour {
		
		public AudioSource gameMusicOne;
		public AudioSource gameMusicTwo;
		public AudioSource gameMusicOneIntense;
		public AudioSource gameMusicTwoIntense;

		public void  Start (){
			// remember volume level from last time
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicOne.volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicTwo.volume = PlayerPrefs.GetFloat("MusicVolume");
		}

		public void UpdateVolume (){
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicOne.volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicTwo.volume = PlayerPrefs.GetFloat("MusicVolume");
		}
	}
}