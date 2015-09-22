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
	private float m_turnForce;		// Force applied for turning
	private float m_turnDirection;	// The direction to turn
	
	private Rigidbody2D m_rigidbody;
	
	///
	/// Properties for access to private members
	///
	public bool Accelerate
	{
		get { return m_accelerate; }
		set { m_accelerate = value; }
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
				
				// So the length of the trail is shorter at lower speeds
				thrustParticleSystem.startSpeed = m_maxMoveSpeed * m_accelPercent;
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
	public void Init( float accelForce, float maxMoveSpeed, float turnForce, float maxTurnSpeed )
	{
		float mass = m_rigidbody.mass;
		float radius = 1;
		
		CircleCollider2D circleCol = GetComponent<CircleCollider2D>();

		if( circleCol != null )
		{
			radius = circleCol.radius;
		}
		
		m_maxMoveSpeed = maxMoveSpeed;
		
		m_accelForce = accelForce * mass;
		m_turnForce = turnForce * mass * radius * radius * 0.5f;

		// Sets the drag to limit the maximum speed
		// Might have to change if we want less drag for more floaty movement
		m_rigidbody.drag = accelForce / maxMoveSpeed;
		m_rigidbody.angularDrag = turnForce / ( maxTurnSpeed * Mathf.Deg2Rad );
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
	
	///
	/// Private Methods
	///
	private void ApplyAcceleration()
	{
		m_rigidbody.AddForce( transform.up * m_accelForce * m_accelPercent, ForceMode2D.Force );
	}
	
	private void Turn()
	{
		m_rigidbody.AddTorque( m_turnForce * m_turnDirection, ForceMode2D.Force );
	}
}
