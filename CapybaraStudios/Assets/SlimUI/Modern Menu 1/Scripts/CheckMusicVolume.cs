using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckMusicVolume : MonoBehaviour {
		
		public AudioSource gameMusicOne;
		public AudioSource gameMusicTwo;
		public AudioSource gameMusicOneIntense;
		public AudioSource gameMusicTwoIntense;
		public AudioSource endMusic;

		public void  Start (){
			// remember volume level from last time
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicOne.volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicTwo.volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicOneIntense.volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicTwoIntense.volume = PlayerPrefs.GetFloat("MusicVolume");
			endMusic.volume = PlayerPrefs.GetFloat("MusicVolume");
		}

		public void UpdateVolume (){
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicOne.volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicTwo.volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicOneIntense.volume = PlayerPrefs.GetFloat("MusicVolume");
			gameMusicTwoIntense.volume = PlayerPrefs.GetFloat("MusicVolume");
			endMusic.volume = PlayerPrefs.GetFloat("MusicVolume");
		}
	}
}