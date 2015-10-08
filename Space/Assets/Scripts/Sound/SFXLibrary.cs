using UnityEngine;
using System.Collections.Generic;

public class SFXLibrary
{
	private static SFXLibrary ms_instance;

	private Dictionary<string, AudioClip> sounds;

	public static SFXLibrary Instance
	{
		get
		{
			if( ms_instance == null )
			{
				ms_instance = new SFXLibrary();
			}
			return ms_instance;
		}
	}

	public SFXLibrary()
	{
		InitLibrary();
	}

	public static AudioClip GetSound( string name )
	{
		AudioClip sound;
		if ( Instance.sounds.TryGetValue( name, out sound ) )
		{
			return sound;
		}
		else
		{
			Debug.Log( "WARNING: Tom hasn't created " + name + " yet, you immpatient bastard." );
			return null;
		}
	}

	private void InitLibrary()
	{
		sounds = new Dictionary<string, AudioClip>();
		Object[] soundsObjs = Resources.LoadAll( "SFX" );

		for( int i = 0; i < soundsObjs.Length; i++ )
		{
			sounds.Add( soundsObjs[ i ].name, (AudioClip)soundsObjs[ i ] );
		}
	}
}
