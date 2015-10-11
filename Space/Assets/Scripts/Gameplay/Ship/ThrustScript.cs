using UnityEngine;

// Zoom zoom
public class ThrustScript : MonoBehaviour
{
	///
	/// Public members to be assigned in the Inpector
	///
	public ParticleSystem thrustParticleSystem;
	
	///
	/// Private members
	///
	private bool m_accelerate;		// Is the pedal to the metal?
	
	private float m_accelPercent;	// 0 to 1 percent of the acceleration to apply
	private float m_accelForce;		// Force applied when accelerating
	private float m_maxMoveSpeed;	// Max movement speed
	private float m_maxTurnSpeed;
	private float m_turnForce;		// Force applied for turning
	private float m_turnDirection;	// The direction to turn
	private float m_brakingDrag;
	
	private Rigidbody2D m_rigidbody;
	private SoundSystemScript m_soundSystem;
	
	///
	/// Properties for access to private members
	///
	public bool Accelerate
	{
		get { return m_accelerate; }
		set
		{ 
			m_accelerate = value;
			ToggleSound();
		}
	}
	
	public float TurnDirection
	{
		get { return m_turnDirection; }
		set { m_turnDirection = Mathf.Clamp( value, -1.0f, 1.0f ); }
	}
	
	public float AccelPercent
	{
		get { return m_accelPercent; }
		set { m_accelPercent = Mathf.Clamp( value, 0.0f, 1.0f ); }
	}
	
	///
	/// Monobehavior Methods
	///

	void Awake ()
	{
		m_accelerate = false;
		m_turnDirection = 0;
		m_accelPercent = 1.0f;
		
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_rigidbody.centerOfMass = Vector2.zero;
		m_soundSystem = GetComponent<SoundSystemScript>();
	}
	
	// Using FixedUpdate here cause that's how Unity do
	// Forces applied accross multiple frames need to be done in FixedUpdate for consistency
	void FixedUpdate()
	{
		if( m_accelerate )
		{
			ApplyAcceleration();
			
			if( thrustParticleSystem != null )
			{
				thrustParticleSystem.Play();
			}
		}
		else if( thrustParticleSystem != null )
		{
			thrustParticleSystem.Stop();
		}
		
		if( m_turnDirection != 0 )
		{
			Turn();
		}
	}
	
	///
	/// Public Methods
	///
	
	// Sets the values of some members
	// We might just want properties for each member later on down the (intergalactic) road
	// MoveSpeed is in units/sec, TurnSpeed is in deg/sec
	public void Init( float accelForce, float maxMoveSpeed, float turnForce )
	{
		float mass = m_rigidbody.mass;
		
		m_maxMoveSpeed = maxMoveSpeed;
		
		m_accelForce = accelForce * mass;
		m_turnForce = turnForce * mass;

		m_brakingDrag = 1;//m_accelForce / maxMoveSpeed;
		//m_rigidbody.angularDrag = turnForce / ( maxTurnSpeed * Mathf.Deg2Rad );
	}
	
	// Adds a given impulse force to the center of the ship (ie recoil or something)
	public void AppyImpulse( Vector2 force )
	{
		m_rigidbody.AddForce( force, ForceMode2D.Impulse );
	}
	
	// Overload! Adds a given impulse to a given point on the ship (ie hit with a missile)
	public void AppyImpulse( Vector2 force, Vector2 position )
	{
		m_rigidbody.AddForceAtPosition( force, position, ForceMode2D.Impulse );
	}

	public void EnableBrake( bool enabled )
	{
		m_rigidbody.drag = enabled ? m_brakingDrag : 0;
	}
	
	///
	/// Private Methods
	///
	private void ApplyAcceleration()
	{
		Vector2 velocity = m_rigidbody.velocity;
		m_rigidbody.AddForce( transform.up * m_accelForce * m_accelPercent, ForceMode2D.Force );

		float speed = velocity.magnitude;

		if( speed > m_maxMoveSpeed * m_accelPercent )
		{
			float velocityDiff = velocity.magnitude - ( m_maxMoveSpeed - 1 ) * m_accelPercent;
			Vector2 counterForce = velocity.normalized * m_accelForce * velocityDiff;
			m_rigidbody.AddForce( -counterForce, ForceMode2D.Force );
		}
	}
	
	private void Turn()
	{
		m_rigidbody.AddTorque( m_turnForce * m_turnDirection, ForceMode2D.Force );
	}

	private void ToggleSound()
	{
		if( m_accelerate )
		{
			if( !m_soundSystem.IsPlaying() )
			{
				m_soundSystem.PlayLooping( "Ship_Engine" );
			}
		}
		else
		{
			m_soundSystem.StopPlaying();
		}
	}
}
