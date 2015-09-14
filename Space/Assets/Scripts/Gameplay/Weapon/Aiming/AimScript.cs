using UnityEngine;

// Script to handle pointing the shooty end of a weapon at a target
// Should be used with a TargetingBehvior
public class AimScript : MonoBehaviour
{
	///
	/// Public members to be assigned in the Inpector
	///
	public bool immediateAim = false; 			// Should aiming be immediate?
	public bool independentRotation = false; 	// Should the weapon rotate independently from the ship?
	
	public float rotationSpeed = 10.0f;			// Speed of roation in degrees/sec
	
	///
	/// Private members
	///
	private Vector3 m_targetPosition;	// I'll give you one guess
	private Quaternion m_rotation;		// Used for independent rotation
	
	///
	/// Monobehavior Methods
	///
	void Start()
	{
		// initializing target pos to straight ahead just incase it isn't set
		m_targetPosition = transform.position + transform.up;
		m_rotation = transform.rotation;
	}
	
	void Update ()
	{
		if( independentRotation )
		{
			transform.rotation = m_rotation;
		}
		
		if( immediateAim )
		{
			ImmediateAim();
		}
		else
		{
			Aim();
		}
	}
	
	// LateUpdate is like if normal update woke up late
	void LateUpdate()
	{
		if( independentRotation )
		{
			m_rotation = transform.rotation;
		}
	}
	
	///
	/// Public Methods
	///
	public void SetTarget( Vector3 targetPos )
	{
		m_targetPosition = targetPos;
	}
	
	///
	/// Private Methods
	///
	private void Aim()
	{
		Vector3 toTarget = m_targetPosition - transform.position;
		Quaternion desiredRot = Quaternion.LookRotation( Vector3.forward, toTarget );
		transform.rotation = Quaternion.RotateTowards( transform.rotation, desiredRot, rotationSpeed * Time.deltaTime );
	}
	
	private void ImmediateAim()
	{
		// It's worth noting that the Up axis is essentially 2D's Forward
		// (The params for LookRotation are forward and up, the second param is usually the constant in 3D)
		transform.rotation = Quaternion.LookRotation( Vector3.forward, m_targetPosition - transform.position );
	}
}
