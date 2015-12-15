using UnityEngine;
using System.Collections.Generic;

// Set an open course for the virgin sea
public class PlayerShipScript : ShipScript
{
	public static PlayerShipScript player = null;
	
	public GameObject objectiveMarker;
	public GameObject stationMarker;

	public bool devGod;
	
	public bool Alive
	{
		get{ return m_alive; }
	}
	
	private Transform m_cameraTransform;
	private bool m_docked = false;
	private bool m_alive = true;
	
	public GameObject ObjectiveMarker
	{
		get{ return objectiveMarker; }
	}
	
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
		player = this;

		m_alive = true;
		
		InitShip();
		
		// The camera is parented to a GO and offset on the Z axis
		// We're keeping the parent so we don't have to set the Z when moving the camera
		m_cameraTransform = Camera.main.transform.parent;
	}
	
	void Start()
	{
		m_thrust.Init( accelForce, maxMoveSpeed, turnForce );
		
		if( m_shield != null )
		{
			m_shield.SetAsPlayerShield();
		}
		// The camera is parented to a GO and offset on the Z axis
		// We're keeping the parent so we don't have to set the Z when moving the camera
		m_cameraTransform = Camera.main.transform.parent;

		SetDefaultLoadout ();
		GameMaster.playerData.playerMoney = 25;
	}
	
	void Update ()
	{
		// Keeping the camera with us
		m_cameraTransform.position = transform.position;
		
		//Don't fire or move if we're docked or dead
		if(!m_docked && m_alive && GameMaster.CurrentGameState != GameState.Warping)
		{
			// Giving input to the thrust 
			m_thrust.Accelerate = ( Input.GetAxis( "Vertical" ) > 0 );
			m_thrust.TurnDirection = -Input.GetAxis( "Horizontal" );
			
			if( Input.GetAxisRaw( "Vertical" ) < 0 )
			{
				m_thrust.EnableBrake( true );
			}
			else
			{
				m_thrust.EnableBrake( false );
			}
			
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
		InitWeapons();
	}

	public void Repair()
	{
		m_health = m_maxHealth;
		EventManager.TriggerEvent (EventDefs.PLAYER_HEALTH_UPDATE);
	}
	
	public override void ApplyDamage( float damage, float shieldPen = 0.0f )
	{
		if(!devGod)
		{
			base.ApplyDamage( damage, shieldPen );
			EventManager.TriggerEvent( EventDefs.PLAYER_HEALTH_UPDATE );
		}
	}
	
	// Checks if any of the number keys were pressed to toggle weapons
	private void SetActiveWeapons()
	{
		for ( int i = 0; i < m_weaponSlots.Length; i++ )
		{
			if ( Input.GetKeyDown( "" + ( i + 1 ) ) )
			{
				if( m_weaponSlots[ i ].Weapon != null )
				{
					m_weaponSlots[ i ].Weapon.ToggleActive();

					UI_Manager.instance.weaponDockUI.GetComponent<WeaponDock>().ToggleWeaponColor( i );
				}
			}
		}
	}

	private void SetDefaultLoadout()
	{
		GameObject g = GameMaster.WeaponMngr.GetWeaponPrefab ( WeaponScript.WeaponType.LASER_MACHINE_GUN );
		WeaponModifier.ModifierNames mod = WeaponModifier.ModifierNames.Crappy;
		WeaponInfo wep1 = g.GetComponent<WeaponScript> ().ToInfo ( mod );

		g = GameMaster.WeaponMngr.GetWeaponPrefab ( WeaponScript.WeaponType.SNIPER );
		WeaponInfo wep2 = g.GetComponent<WeaponScript> ().ToInfo ( mod );

		g = GameMaster.WeaponMngr.GetWeaponPrefab ( WeaponScript.WeaponType.BEAM );
		WeaponInfo wep3 = g.GetComponent<WeaponScript> ().ToInfo ( mod );

		m_weaponSlots [0].SetWeapon (wep3.SpawnWeapon ());
		m_weaponSlots [1].SetWeapon (wep2.SpawnWeapon ());
		m_weaponSlots [2].SetWeapon (wep1.SpawnWeapon ());
		UI_Manager.instance.UpdateWeaponDockUI ();
	}
	
	protected override void Die()
	{
		//can't die more than once
		if(!m_alive)
			return;
		
		m_alive = false;
		//Kill thrusters
		if(m_thrust)
		{
			m_thrust.Accelerate = false;
			m_thrust.TurnDirection = 0;
		}
		
		GameMaster.CurrentGameState = GameState.GameOver;
		UI_Manager.instance.ChangeUIState (GameState.GameOver);

		base.Die();
	}
}