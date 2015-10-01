using UnityEngine;
using System.Collections;

[ RequireComponent (typeof ( AudioSource ) ) ]

public class SoundSystemScript : MonoBehaviour
{
	private AudioSource m_audioSrc;

	void Awake()
	{
		m_audioSrc = GetComponent<AudioSource>();
		m_audioSrc.spatialBlend = 1.0f; // Just in case
	}

	public void Play()
	{
		m_audioSrc.Play();
	}

	public void PlayOneShot( string soundName, float volume = 1.0f, float pitch = 1.0f )
	{
		m_audioSrc.loop = false;
		m_audioSrc.pitch = pitch;
		m_audioSrc.PlayOneShot( SFXLibrary.GetSound( soundName ), volume );
	}

	public void PlayLooping( string soundName )
	{
		m_audioSrc.loop = true;
		SetSound( soundName );
		m_audioSrc.Play();
	}

	public void StopPlaying()
	{
		m_audioSrc.Stop();
	}

	public void SetSound( string soundName )
	{
		m_audioSrc.clip = SFXLibrary.GetSound( soundName );
	}

	public bool IsPlaying()
	{
		return m_audioSrc.isPlaying;
	}
}
