using UnityEngine;

/**
 * The audio manager provides a simple audio playback for objects that will be deactivated but still has to play a sound
 */
public class AudioManager : Singleton<AudioManager>
{
	/**
	 * The audio managers audio source
	 */
	private AudioSource audioSource;


	/**
	 * Load the audio source component
	 * It's used to playback audio even if the game object is disabled (For example: Destroied asteroids or aliens)
	 */
	private void Start()
	{
		this.audioSource = this.GetComponent<AudioSource>();
	}


	/**
	 * Simply plays the given audio clip
	 */
	public void Play(AudioClip audioClip)
	{
		this.audioSource.PlayOneShot(audioClip);
	}
}
