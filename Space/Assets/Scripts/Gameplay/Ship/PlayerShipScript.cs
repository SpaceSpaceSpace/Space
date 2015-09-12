using UnityEngine;
using System.Collections.Generic;

// Set an open course for the virgin sea
public class PlayerShipScript : ShipScript
{
	public List<Vector2> AttachmentPoints = new List<Vector2>();
	public Dictionary<Vector2, GameObject> Attachments = new Dictionary<Vector2, GameObject>();

	private Transform m_cameraTransform;
	private bool m_docked;
	
	void Start ()
	{
		InitShip();
		m_thrust.Init( 50.0f, 5.0f, 10.0f, 90.0f ); // Magic numbers. Because I can.
		
		// The camera is parented to a GO and offset on the Z axis
		// We're keeping the parent so we don't have to set the Z when moving the camera
		m_cameraTransform = Camera.main.transform.parent;
		
		// Temporarily just grabbing the weapons from the children
		// Children shouldn't have weapons anyway. They're children.
		int numNonWepChildren = 2;
		m_weapons = new WeaponScript[ transform.childCount - numNonWepChildren ];
		for( int i = numNonWepChildren; i < transform.childCount; i++ )
		{
			m_weapons[ ( i - numNonWepChildren ) ] = transform.GetChild( i ).GetComponent<WeaponScript>();
		}
	}
	
	void Update ()
	{
		// Keeping the camera with us
		m_cameraTransform.position = transform.position;

		//Don't fire or move if we're docked
		if(!m_docked)
		{
			// Giving input to the thrust 
			m_thrust.Accelerate = ( Input.GetAxis( "Vertical" ) > 0 );
			m_thrust.TurnDirection = -Input.GetAxis( "Horizontal" );
			
			// If a key was pressed, might as well check if it was a number key
			if( Input.anyKeyDown )
			{
				SetActiveWeapons();
			}
			
			// Doing the pew pew
			if( Input.GetButton( "Fire1" ) )
			{
				FireWeapons();
			}
			else if( Input.GetButtonUp( "Fire1" ) )
			{
				ReleaseFire();
			}
		}
	}
	
	// Checks if any of the number keys were pressed to toggle weapons
	private void SetActiveWeapons()
	{
		for ( int i = 0; i < m_weapons.Length; i++ )
		{
			if ( Input.GetKeyDown( "" + ( i + 1 ) ) )
			{
				m_weapons[ i ].ToggleActive();
			}
		}
	}
}
