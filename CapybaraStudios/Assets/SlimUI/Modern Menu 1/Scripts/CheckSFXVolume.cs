using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckSFXVolume : MonoBehaviour {

		// public AudioSource ak47_reload;
		// public AudioSource ak47_shot;
		// public AudioSource button;
		// public AudioSource dead;
		// public AudioSource door;
		// public AudioSource grappling_grab;
		// public AudioSource grappling_reload;
		// public AudioSource grappling_shoot;
		// public AudioSource heal;
		// public AudioSource jump;
		// public AudioSource kill;
		// public AudioSource knife;
		// public AudioSource lmg_shot;
		// public AudioSource pickup;
		// public AudioSource pistol_reload;
		// public AudioSource pistol_shot;
		// public AudioSource portal;
		// public AudioSource reload;
		// public AudioSource shell;
		// public AudioSource shells;
		// public AudioSource shotgun_reload;
		// public AudioSource shotgun_shot;
		// public AudioSource slide;
		// public AudioSource sniper_reload;
		// public AudioSource sniper_scope;
		// public AudioSource sniper_shot;
		// public AudioSource steps;
		// public AudioSource uzi_shot;
		public AudioSource[] sounds;

		public void  Start () {
			// remember volume level from last time
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume");
			Debug.Log(PlayerPrefs.GetFloat("SFXVolume"));

			foreach(AudioSource sound in sounds)
			{
				sound.volume = PlayerPrefs.GetFloat("SFXVolume");
			}
		}

		public void UpdateVolume (){
			GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume");
			foreach(AudioSource sound in sounds)
			{
				sound.volume = PlayerPrefs.GetFloat("SFXVolume");
			}
		}
	}
}