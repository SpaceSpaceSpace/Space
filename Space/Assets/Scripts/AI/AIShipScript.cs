using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// This script contains all of the necessary methods to carry out
// behaviours in a ShipBehaviourScript
public class AIShipScript : ShipScript {

	///
	/// Public Variables
	///
	public List<GameObject> squad; // the squad of ships
	public GameObject leader; // the leader of a squad
	public Transform player;
	public Transform objective;
	public bool aggro; // is the enemy in combat

	///
	/// Private Variables
	///
	private Transform m_target; // the transform of the ship's target, currently the player
	private int passSide; // is the side for the ship to pass on set
	private float wanderAngle;
	public bool m_obstacle; // is there an obstacle in the way

	// Weights for flocking
	private const float ALIGNMENT = 4.0f;
	private const float SEPARATION = 6.0f;
	private const float COHESION = 1.0f;
	// The distance for separation
	private const float SEP_DISTANCE = 3.0f;



	// Acessors
	public Transform Target { get { return m_target; } set { m_target = value; } }
	public bool Obstacle { get { return m_obstacle; } }
	// Use this for initialization
	void Start () {
		InitShip();
		m_thrust.Init(accelForce, maxMoveSpeed, turnForce);

		player = GameObject.Find("Player Ship").transform;
		m_target = GameObject.Find("Player Ship").transform; // Find the player, will likely change
		passSide = -1;
		wanderAngle = 0.0f;
		m_thrust.AccelPercent = 1.0f;
		m_thrust.Accelerate = true;
		aggro = false;

		for(int i = 0; i < squad.Count; i++)
		{
			if(squad[i].GetComponent<ShipBehaviourScript>().behaviour.ToString() == "Leader")
				leader = squad[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		DetectObstacle();
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
	public void MoveToward(Transform target)
	{
		m_thrust.AccelPercent = 1.0f;
		Vector2 targetPos = Vector2.zero;
		if(Vector2.Distance(target.position, transform.position) > 5.0f)
			targetPos = PredictPosition(target, maxMoveSpeed);
		else
			targetPos = target.position;
		FaceTarget(targetPos);

		if(Vector2.Angle(targetPos - (Vector2)transform.position, transform.up) < 45)
			m_thrust.Accelerate = true;
	}

	// Flee directly from the target
	public void MoveAwayFrom(Transform target)
	{
		m_thrust.AccelPercent = 1.0f;
		Vector2 targetPos = transform.position + (transform.position - target.position);
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

	public bool CheckAggro()
	{
		if(DistanceTo(Target.position) < 10.0f)
		{
			aggro = true;
			return true;
		}
		else
			aggro = false;
		foreach(GameObject g in squad)
		{
			if(g.GetComponent<AIShipScript>().aggro && g != this.gameObject)
				return true;
		}
		return false;

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
		if(distance < minDistance)
		{
			m_thrust.AccelPercent -= 1.0f * Time.deltaTime;
		}
		else if(distance > maxDistance)
			m_thrust.AccelPercent += 1.0f * Time.deltaTime;
		else
		{
			float targetSpeed = m_target.GetComponent<Rigidbody2D>().velocity.magnitude;
			m_thrust.AccelPercent = maxMoveSpeed / targetSpeed;
		}
	}


	public float DistanceTo(Vector2 target)
	{
		return Vector2.Distance(transform.position, target);
	}

	public void FireWeapon(int index)
	{
		if(m_weapons.Length > index)
		{
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

	// flock with the other ships in the squad
	public void Flock()
	{
		m_thrust.Accelerate = true;
		Vector2 align = Vector2.zero; // the alignment angle of the squad
		Vector2 center = Vector2.zero; // the center of the squad
		Vector2 separation = Vector2.zero; // the closest ship in the squad
		// Gather all of the data necessary to figure out the flocking vectors
		foreach(GameObject g in squad)
		{
			center += (Vector2)g.transform.position;
			float dist = Vector2.Distance(g.transform.position, transform.position);
			if(g != this.gameObject && dist < SEP_DISTANCE)
			{
				Vector2 fromShip = (Vector2)(transform.position - g.transform.position);
				fromShip *= SEP_DISTANCE/fromShip.magnitude;
				separation += fromShip;
			}
			align += (Vector2)(g.transform.up - transform.up);
		}

		align /= squad.Count;
		center /= squad.Count;
		align = align.normalized * ALIGNMENT;
		Vector2 cohesion = center - (Vector2) transform.position;
		cohesion = cohesion.normalized * COHESION;

		separation = separation.normalized * SEPARATION;

		Vector2 final = separation + cohesion + align;
		if(Vector2.Dot(final, transform.right) > 0.1f)
			m_thrust.TurnDirection = -1;
		else if(Vector2.Dot(final, transform.right) < 0.1f)
			m_thrust.TurnDirection = 1;
		else
			m_thrust.TurnDirection = 0;
		
		if(Vector2.Dot(final, transform.up) > 0)
			m_thrust.AccelPercent += 1.0f * Time.deltaTime;
		else
			m_thrust.AccelPercent -= 1.0f * Time.deltaTime;

		// Set a minimum speed for the squad
		if(m_thrust.AccelPercent < 0.4f)
			m_thrust.AccelPercent = 0.4f;

	}

	public bool CanSeeTarget()
	{
		float targetDist = Vector2.Distance(m_target.position, transform.position);
		if(targetDist > 15.0f)
			return false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, m_target.position - transform.position, 15.0f);
		if(hit && hit.collider.gameObject.name == "Player Ship")
			return true;

		return false;
	}

	public void AvoidObstacle()
	{
		m_obstacle = false;
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, GetComponent<CircleCollider2D>().radius, transform.up, 10.0f);
		float closestDist = 100.0f; // the closest gameobject
		Vector2 closestPos = Vector2.zero; // the center of the closest obstacle
		Vector2 impactPoint = Vector2.zero; // the point of impact for the closest obstacle
		foreach(RaycastHit2D h in hits)
		{
			if(h.collider.gameObject.tag == "Asteroid")
			{
				m_obstacle = true;
				Vector2 obsPos = h.collider.gameObject.transform.position;
				if(h.distance < closestDist)
				{
					closestPos = obsPos;
					impactPoint = h.point;
				}
			}
		}

		if(!m_obstacle)
			return;
		
		// turn to go around the obstacle
		if(Vector2.Dot(transform.right, closestPos - (Vector2)transform.position) > 0)
			m_thrust.TurnDirection = 1;
		else
			m_thrust.TurnDirection = -1;

		float impactDist = Vector2.Distance(impactPoint, transform.position);
		// slow down when getting too close to the obstacle
		if(impactDist < GetComponent<CircleCollider2D>().radius + (maxMoveSpeed * m_thrust.AccelPercent))
			m_thrust.AccelPercent -= 1.0f * Time.deltaTime;

		if(m_thrust.AccelPercent < 0.1f)
			m_thrust.AccelPercent = 0.1f;

		// stop completely if about to impact
		if(impactDist < GetComponent<CircleCollider2D>().radius + 0.25f)
			m_thrust.Accelerate = false;
		

	}

	public void DetectObstacle()
	{
		m_obstacle = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 10.0f);


		if(hit && hit.collider.gameObject.tag == "Asteroid")
		{
			m_obstacle = true;
		}
	}



	// return the angle between the direction the AI ship is facing
	// and the direction to the target's predicted position
	public float AngleToTarget()
	{
		Vector2 target = m_target.position - transform.position;
		float angle = Vector2.Angle(transform.up, target);
		return angle;
	}

	// Using the target's velocity, a parameter velocity, and the distance to the target,
	// predict where the interception point of the target and the speed provided
	Vector2 PredictPosition(Transform target, float speed)
	{
		// if the target doesn't have a rigidbody we can't get its velocity,
		// so return the target's position
		if(!target.GetComponent<Rigidbody2D>() || target.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f) 
			return target.position;

		Vector2 toTarget = target.position - transform.position; // Vector to the target
		Vector2 targetVel = target.GetComponent<Rigidbody2D>().velocity; // the velocity of the target
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
			return target.position;
		float targetAngle = Mathf.Asin(velRatio * targetVel.magnitude); // Get the second angle using the velRatio and the vel of the target
		float finalAngle = Mathf.PI - (angle + targetAngle); // Find the final angle of the triangle, the one opposite the distance
		float distRatio = distance / Mathf.Sin(finalAngle); // Now get the Law of Sins ratio, but we can do it with the distance now
		float predictDist = distRatio * Mathf.Sin(angle); // Find the distance this ship will have to travel to intercept the target
		float time =  Mathf.Abs(predictDist / speed); // and use it to determine the amount of time that will take
		// And now we have the predicted position by advancing the target's position by the time 
		Vector2 predictPos = (Vector2)target.position + (targetVel * time); 

		Debug.DrawLine(transform.position, predictPos);

		return predictPos;
	}

	void OnDestroy()
	{
		squad.Remove (this.gameObject);
	}
}
