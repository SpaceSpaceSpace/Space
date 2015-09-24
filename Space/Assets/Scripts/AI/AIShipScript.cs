using UnityEngine;
using System.Collections;

// This script contains all of the necessary methods to carry out
// behaviours in a ShipBehaviourScript
public class AIShipScript : ShipScript {

	///
	/// Public Variables
	///
	public float accelForce; // the accel force for thrust
	public float turnForce; // the turn force for thrust
	public float maxMoveSpeed; // max move speed for thrust
	public float maxTurnSpeed; // max turn speed for thrust

	///
	/// Private Variables
	///
	private Transform m_target; // the transform of the ship's target, currently the player
	private int passSide; // is the side for the ship to pass on set
	private float wanderAngle;


	// Acessors
	public Transform Target {get {return m_target;}}
	// Use this for initialization
	void Start () {
		InitShip();
		m_thrust.Init(accelForce, maxMoveSpeed, turnForce, maxTurnSpeed);

		m_target = GameObject.FindWithTag("Player").transform; // Find the player, will likely change
		passSide = -1;
		wanderAngle = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	}


	// Turn to face the target
	void FaceTarget(Vector2 targetPos) {
		if(Vector2.Angle(targetPos - (Vector2)transform.position, transform.up) > 5)
		{
			float dot = Vector2.Dot(transform.right, targetPos - (Vector2)transform.position);
			if(dot > 0)
				m_thrust.TurnDirection = -1;
			else if(dot < 0)
				m_thrust.TurnDirection = 1;
		}
		else
			m_thrust.TurnDirection = 0;

	}

	// Move toward the target's predicted position
	public void MoveTowardTarget()
	{
		m_thrust.AccelPercent = 1.0f;
		Vector2 targetPos = PredictTargetPosition(maxMoveSpeed);
		FaceTarget(targetPos);

		if(Vector2.Angle(targetPos - (Vector2)transform.position, transform.up) < 45)
			m_thrust.Accelerate = true;
	}

	// Flee directly from the target
	public void MoveAwayFromTarget()
	{
		m_thrust.AccelPercent = 1.0f;
		Vector2 targetPos = transform.position + (transform.position - m_target.position);
		FaceTarget(targetPos);
		m_thrust.Accelerate = true;
	}

	// As it says, go forward, full speed
	public void MoveForward()
	{
		m_thrust.AccelPercent = 1.0f;
		FaceTarget(transform.position + transform.up);
		m_thrust.Accelerate = true;
	}

	// pass by the target to the left or right (randomly determined) at the distance input
	public void PassByTarget(float distance)
	{
		m_thrust.AccelPercent = 1.0f;
		Vector2 targetPos = m_target.position;

		if(passSide == -1)
		{
			passSide = Random.Range(0, 11);
		}
		else
		{
			Vector2 toTarget = m_target.position - transform.position;
			toTarget.Normalize();
			if(passSide > 5)
				targetPos += new Vector2(-toTarget.y, toTarget.x) * distance;
			else
				targetPos -= new Vector2(toTarget.y, -toTarget.x) * distance;
		}

		FaceTarget(targetPos);
		m_thrust.Accelerate = true;


	}
	 
	// Reset which side this ship will pass the target on
	public void ResetPassSide()
	{
		passSide = -1;
	}

	// Follow the target, staying in between the max distance and min distance
	public void ChaseTarget(float maxDistance, float minDistance)
	{
		FaceTarget(m_target.position);

		float distance = Vector2.Distance(m_target.position, transform.position);
		if(distance < minDistance || AngleToTarget() > 45)
			m_thrust.AccelPercent -= 1.0f * Time.deltaTime;
		else if(distance > maxDistance)
			m_thrust.AccelPercent += 1.0f * Time.deltaTime;
	}

	public float DistanceToTarget()
	{
		return Vector2.Distance(transform.position, m_target.position);
	}

	public void FireWeapon(int index)
	{
		if(m_weapons.Length > index)
		{
			m_weapons[index].Active = true;
			m_weapons[index].Fire();
		}
	}

	public void Wander()
	{
		wanderAngle += Random.Range(-0.05f, 0.05f);
		Vector2 wanderPos = 3.0f * new Vector2(Mathf.Cos(wanderAngle), Mathf.Sin(wanderAngle));
		wanderPos += (Vector2)(transform.position + (transform.up * 5.0f));
		m_thrust.AccelPercent = 0.5f;
		FaceTarget(wanderPos);
		m_thrust.Accelerate = true;

	}

	// return the angle between the direction the AI ship is facing
	// and the direction to the target's predicted position
	float AngleToTarget()
	{
		Vector2 target = m_target.position - transform.position;
		float angle = Vector2.Angle(transform.up, target);
		return angle;
	}

	// Using the target's velocity, a parameter velocity, and the distance to the target,
	// predict where the interception point of the target and the speed provided
	Vector2 PredictTargetPosition(float speed)
	{
		// if the target doesn't have a rigidbody we can't get its velocity,
		// so return the target's position
		if(!m_target.GetComponent<Rigidbody2D>() || m_target.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f) 
			return m_target.position;

		Vector2 toTarget = m_target.position - transform.position; // Vector to the target
		Vector2 targetVel = m_target.GetComponent<Rigidbody2D>().velocity; // the velocity of the target
		float angle = 180 - Vector2.Angle(toTarget, targetVel); // the angle betweeen the target's velocity and the ane vector to the target
		float distance = toTarget.magnitude; // the distance to the target
		// Now for some math stuff
		// This gets hard to follow, but I'm basically creating a triangle.  Two of the sides are represented by 
		// the velocities of the target and this ship, and the other is the distance between this ship and the target.
		// I use the Law of Sins to find the missing angles so I can convert one of the sides which is represented by
		// velocity into distance, and use the now known distance and velocity to determine how much time ahead
		// the predicted position will be.
		angle *= Mathf.Deg2Rad; // convert andgle to radians for sin
		float velRatio =  Mathf.Sin(angle) / speed; // The ratio for the Law of Sins, using the velocity
		if(Mathf.Abs(velRatio *targetVel.magnitude) > 1) // If we can't catch the target, return the target position
			return m_target.position;
		float targetAngle = Mathf.Asin(velRatio * targetVel.magnitude); // Get the second angle using the velRatio and the vel of the target
		float finalAngle = Mathf.PI - (angle + targetAngle); // Find the final angle of the triangle, the one opposite the distance
		float distRatio = distance / Mathf.Sin(finalAngle); // Now get the Law of Sins ratio, but we can do it with the distance now
		float predictDist = distRatio * Mathf.Sin(angle); // Find the distance this ship will have to travel to intercept the target
		float time =  Mathf.Abs(predictDist / speed); // and use it to determine the amount of time that will take
		// And now we have the predicted position by advancing the target's position by the time 
		Vector2 predictPos = (Vector2)m_target.position + (targetVel * time); 

		Debug.DrawLine(transform.position, predictPos);

		return predictPos;
	}
}
