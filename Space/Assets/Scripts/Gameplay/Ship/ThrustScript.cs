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
	
	private float m_accelForce;		// Force applied when accelerating
	private float m_turnForce;		// Force applied for turning
	private float m_turnDirection;	// The direction to turn (ideally should be -1 to 1)
	
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
		set { m_turnDirection = value; }
	}
	
	///
	/// Monobehavior Methods
	///
	
	// You know what Start does, right?
	void Start ()
	{
		m_accelerate = false;
		m_turnDirection = 0;
		
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
		m_accelForce = accelForce;
		m_turnForce = turnForce;
		
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
		m_rigidbody.AddForce( transform.up * m_accelForce, ForceMode2D.Force );
	}
	
	private void Turn()
	{
		m_rigidbody.AddTorque( m_turnForce * m_turnDirection, ForceMode2D.Force );
	}
}
