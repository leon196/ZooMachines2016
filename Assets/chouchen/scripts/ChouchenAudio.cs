using UnityEngine;
using System.Collections;

public class ChouchenAudio : MonoBehaviour {

	public AudioSource _audio;

	public AudioClip clipIntro;

	public AudioClip clipGame;

	// Use this for initialization
	void Start () 
	{
		_audio.clip = clipIntro;
		_audio.Play ();

		StartCoroutine (PlayAfter ());
	}

	IEnumerator PlayAfter()
	{
		yield return new WaitForSeconds (15.6f);

		_audio.Stop ();
		_audio.clip = clipGame;
		_audio.loop = true;
		_audio.Play ();
	}

}
