using UnityEngine;

using System.Collections.Generic;
using UnityEngine.Events;


public class EventManager : MonoBehaviour
{
	private static EventManager ms_instance;
	
	private Dictionary<string, UnityEvent> eventDictionary;
	
	public static EventManager Instance
	{
		get
		{
			if( ms_instance == null )
			{
				GameObject go = new GameObject( "Event Manager" );
				ms_instance = go.AddComponent<EventManager>();
				ms_instance.eventDictionary = new Dictionary<string, UnityEvent>();
			}
			return ms_instance;
		}
	}
	
	void Awake()
	{
		if( ms_instance == null )
		{
			ms_instance = this;
		}
	}

	void OnDestroy()
	{
		// Null out the instance when a new scene is loaded
		if( ms_instance == this )
		{
			ms_instance = null;
		}
	}
	
	// Adds a listener to an event
	// The callback is a void return, zero arguement function
	public static void AddEventListener( string eventName, UnityAction callback )
	{
		UnityEvent thisEvent = null;
		if ( Instance.eventDictionary.TryGetValue ( eventName, out thisEvent ) )
		{
			thisEvent.AddListener ( callback );
		}
		else
		{
			thisEvent = new UnityEvent();
			thisEvent.AddListener( callback );
			ms_instance.eventDictionary.Add( eventName, thisEvent );
		}
	}
	
	// Removes a listener from an event
	public static void RemoveEventListener( string eventName, UnityAction callback )
	{
		// Saftey check for destruction (ie changing scenes) 
		if( ms_instance == null )
		{
			// Early return
			return;
		}
		
		UnityEvent thisEvent = null;
		if ( ms_instance.eventDictionary.TryGetValue( eventName, out thisEvent ) )
		{
			thisEvent.RemoveListener( callback );
		}
	}
	
	// Triggers the event, which triggers all listener callbacks
	public static void TriggerEvent( string eventName )
	{
		UnityEvent thisEvent = null;
		if ( Instance.eventDictionary.TryGetValue( eventName, out thisEvent ) )
		{
			thisEvent.Invoke();
		}
	}
}

public struct EventDefs
{
	public const string PLAYER_HEALTH_UPDATE = "PlayerHealthUpdate";
	public const string PLAYER_SHIELD_UPDATE = "PlayerShieldUpdate";
}