using UnityEngine;
using System.Collections.Generic;

// Set an open course for the virgin sea
public class PlayerShipScript : ShipScript
{
	public static PlayerShipScript player;

	public List<Vector2> AttachmentPoints = new List<Vector2>();
	public Dictionary<Vector2, GameObject> Attachments = new Dictionary<Vector2, GameObject>();

	private Transform m_cameraTransform;
	private bool m_docked = false;

	public float Health
	{
		get { return m_health; }
	}
	
	public float MaxHealth
	{
		get { return m_maxHealth; }
	}

	public ShieldScript Shield
	{
		get { return m_shield; }
	}

	void Awake()
	{
		InitShip();
	}

	void Start ()
	{
		m_thrust.Init( 50.0f, 5.0f, 10.0f, 90.0f ); // Magic numbers. Because I can.
		m_shield.SetAsPlayerShield();
		// The camera is parented to a GO and offset on the Z axis
		// We're keeping the parent so we don't have to set the Z when moving the camera
		m_cameraTransform = Camera.main.transform.parent;
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
			
			if( Input.GetKeyDown( KeyCode.LeftBracket ) )
			{
				m_thrust.AccelPercent -= 0.1f;
			}
			
			if( Input.GetKeyDown( KeyCode.RightBracket ) )
			{
				m_thrust.AccelPercent += 0.1f;
			}
		}
	}
	
	void OnCollisionEnter2D( Collision2D collision )
	{
		HandleCollision( collision );
	}
	
	void OnCollisionStay2D( Collision2D collision )
	{
		// Will handle damage from sustained contact
	}

	public void Dock()
	{
		//Kill thrusters
		if(m_thrust)
		{
			m_thrust.Accelerate = false;
			m_thrust.TurnDirection = 0;
		}

		m_docked = true;
	}

	public void Undock()
	{
		m_docked = false;
	}

	public override void ApplyDamage( float damage, float shieldPen = 0.0f )
	{
		base.ApplyDamage( damage, shieldPen );
		EventManager.TriggerEvent( EventDefs.PLAYER_HEALTH_UPDATE );
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

	protected override void Die()
	{
		print( "sry u died :'(" );
	}
}
