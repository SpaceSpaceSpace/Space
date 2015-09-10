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
	// Use this for initialization
	void Start () {
		InitShip();
		m_thrust.Init(accelForce, maxMoveSpeed, turnForce, maxTurnSpeed);

		m_target = GameObject.FindWithTag("Player").transform; // Find the player, will likely change
	}
	
	// Update is called once per frame
	void Update () {
		MoveTowardTarget();
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

	public void MoveTowardTarget()
	{
		Vector2 targetPos = PredictTargetPosition();
		FaceTarget(targetPos);

		if(Vector2.Angle(targetPos - (Vector2)transform.position, transform.up) < 45)
			m_thrust.Accelerate = true;
		//else
			//m_thrust.Accelerate = false;
	}

	public void MoveAwayFromTarget()
	{

	}

	public void PassTarget()
	{

	}

	// return the angle between the direction the AI ship is facing
	// and the direction to the target's predicted position
	float AngleToTarget()
	{
		Vector2 target = m_target.position - transform.position;
		float angle = Vector2.Angle(transform.up, target);
		return angle;
	}

	// Using the target's velocity, this AI ships max velocity, and the distance to the target,
	// predict where the ship will need to go to intercept the target
	Vector2 PredictTargetPosition()
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
		float velRatio =  Mathf.Sin(angle) / maxMoveSpeed; // The ratio for the Law of Sins, using the velocity
		if(Mathf.Abs(velRatio *targetVel.magnitude) > 1)
			return m_target.position;
		float targetAngle = Mathf.Asin(velRatio * targetVel.magnitude); // Get the second angle using the velRatio and the vel of the target
		float finalAngle = Mathf.PI - (angle + targetAngle); // Find the final angle of the triangle, the one opposite the distance
		float distRatio = distance / Mathf.Sin(finalAngle); // Now get the Law of Sins ratio, but we can do it with the distance now
		float predictDist = distRatio * Mathf.Sin(angle); // Find the distance this ship will have to travel to intercept the target
		float time =  Mathf.Abs(predictDist / maxMoveSpeed); // and use it to determine the amount of time that will take
		// And now we have the predicted position by advancing the target's position by the time 
		Vector2 predictPos = (Vector2)m_target.position + (targetVel * time); 

		Debug.DrawLine(transform.position, predictPos);

		return predictPos;
	}
}
