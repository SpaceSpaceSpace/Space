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
	public Transform obstacleTrans;
	public AISpawnerScript spawner;
	public string[] enemies;
	public string[] friends;
	public Sprite destroyedSprite;

	///
	/// Private Variables
	///
	private Transform m_target; // the transform of the ship's target, currently the player
	private Vector2 m_attackPos; // the position the ship will be aiming for when attacking
	private float m_wanderAngle;
	private bool m_obstacle; // is there an obstacle in the way
	private Vector2 objectiveStartPos;

	// information about the objects around the ship
	private List<GameObject> lethalObj; // ships that will kill this one, not including the player
	private List<GameObject> nonLethalObj; // ships that are not friendly, but can't kill this one
	private List<GameObject> friendObj; // friendly ships
	private bool playerTarget; // is the player among potential targets

	private float[] m_weapRange;
	private float[] m_weapSpread;
	private float[] m_weapSpeed;
	private float[] m_weapLifeSpan;

	// Weights for flocking
	private const float ALIGNMENT = 4.0f;
	private const float SEPARATION = 6.0f;
	private const float COHESION = 1.0f;
	// The distance for separation
	private const float SEP_DISTANCE = 3.0f;



	// Acessors
	public Transform Target { get { return m_target; } set { m_target = value; } }
	public bool Obstacle { get { return m_obstacle; } }
	public Vector2 ObjectiveStartPos { get { return objectiveStartPos; } }
	public float[] WeapRange { get { return m_weapRange; } }
	public float[] WeapSpread { get { return m_weapSpread; } }
	// Use this for initialization
	void Start () {
		InitShip();
		m_thrust.Init(accelForce, maxMoveSpeed, turnForce);

		player = PlayerShipScript.player.transform;
		m_wanderAngle = 0.0f;
		m_thrust.AccelPercent = 1.0f;
		Go ();
		aggro = false;
		m_attackPos = Vector2.zero;
		obstacleTrans = null;
		if(objective != null)
			objectiveStartPos = objective.position;

		friendObj = new List<GameObject>();
		lethalObj = new List<GameObject>();
		nonLethalObj = new List<GameObject>();
		playerTarget = false;

		// set up variables for weapons
		m_weapRange = new float[m_weaponSlots.Length];
		m_weapSpeed = new float[m_weaponSlots.Length];
		m_weapSpread = new float[m_weaponSlots.Length];
		m_weapLifeSpan = new float[m_weaponSlots.Length];
		for(int i = 0; i < m_weaponSlots.Length;i++)
		{
			if( m_weaponSlots[i].Weapon != null )
			{
				switch(m_weaponSlots[i].Weapon.weaponType)
				{
				case WeaponScript.WeaponType.SCATTER_SHOT:
				case WeaponScript.WeaponType.LASER_MACHINE_GUN:
				case WeaponScript.WeaponType.SNIPER:
					ProjectileWeaponScript pScript = GetComponentInChildren<ProjectileWeaponScript>();
					m_weapSpeed[i] = pScript.projectileSpeed;
					m_weapLifeSpan[i] = pScript.projectileLifeTime;
					m_weapSpread[i] = pScript.maxSpreadAngle;
					break;
				case WeaponScript.WeaponType.BEAM:
					BeamWeaponScript bScript = GetComponentInChildren<BeamWeaponScript>();
					m_weapLifeSpan[i] = -1;
					m_weapSpread[i] = 0;
					m_weapSpeed[i] = -1;
					m_weapRange[i] = bScript.beamRange;
					break;
				default:
					m_weapLifeSpan[i] = -1;
					m_weapSpread[i] = 0;
					m_weapSpeed[i] = -1;
					m_weapRange[i] = 10;
					break;
				}
			}
		}

		for(int i = 0; i < squad.Count; i++)
		{
			if(squad[i].GetComponent<ShipBehaviourScript>().behaviour.ToString() == "Leader")
				leader = squad[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		DetectObstacle();
		// calculate the weapon range
		Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
		float rangePercent = Vector2.Dot(transform.up,velocity.normalized);
		for(int i = 0; i < m_weaponSlots.Length;i++)
		{
			if(m_weapLifeSpan[i] != -1)
				m_weapRange[i] = (m_weapSpeed[i] + (velocity.magnitude * rangePercent)) * m_weapLifeSpan[i];
		}

		for(int i = 0; i < lethalObj.Count; i++)
		{
			if(lethalObj[i] == null)
			{
				lethalObj.RemoveAt(i);
			}
		}
		for(int i = 0; i < nonLethalObj.Count; i++)
		{
			if(nonLethalObj[i] == null)
			{
				nonLethalObj.RemoveAt(i);
			}
		}
		for(int i = 0; i < friendObj.Count; i++)
		{
			if(friendObj[i] == null)
			{
				friendObj.RemoveAt(i);
			}
		}
	}


	// Turn to face the target
	public void FaceTarget(Vector2 targetPos) {
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

	public void TurnWeapon()
	{
		if(m_weaponSlots[0].Weapon != null)
		{
			float angle = m_weaponSlots[0].Weapon.transform.eulerAngles.z;
			float targetAngle = Vector2.Angle(Vector2.up, m_target.position - transform.position);
			if(Vector2.Dot(Vector2.right, m_target.position - transform.position) > 0)
				targetAngle = 360.0f - targetAngle;
			if(Mathf.Abs(angle - targetAngle) > 5.0f || Mathf.Abs((angle + 360.0f) - targetAngle) > 5.0f)
			{
				if(angle > targetAngle)
				{
					if(angle - targetAngle > 180)
						angle += 180.0f * Time.deltaTime;
					else
						angle -= 180.0f * Time.deltaTime;
				}
				else
				{
					if(targetAngle - angle > 180)
						angle -= 180.0f * Time.deltaTime;
					else
						angle += 180.0f * Time.deltaTime;
				}
			}

			m_weaponSlots[0].Weapon.transform.eulerAngles = new Vector3(0, 0, angle);
		}
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

	public void SelectTarget()
	{
		if(m_target == null)
		{
			// if no player and lethal targets
			if(!playerTarget && lethalObj.Count > 0)
			{
				int index = Random.Range(0, lethalObj.Count - 1);
				if(lethalObj[index] != null)
					m_target = lethalObj[index].transform;
			}
			// if no lethal targets and player
			else if(lethalObj.Count <= 0 && playerTarget)
			{
				m_target = PlayerShipScript.player.transform;
			}
			// if player and lethal targets
			else if(lethalObj.Count > 0 && playerTarget)
			{
				int index = Random.Range(0, lethalObj.Count);
				if(index == lethalObj.Count)
					m_target = PlayerShipScript.player.transform;
				else
					m_target = lethalObj[index].transform;
			}
			// if no player and no lethal targets
			else if(nonLethalObj.Count > 0)
			{
				int index = Random.Range(0, nonLethalObj.Count - 1);
				m_target = nonLethalObj[index].transform;
			}
		}

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

	public void Stop()
	{
		m_thrust.Accelerate = false;
		m_thrust.EnableBrake(true);
	}

	public void Go()
	{
		m_thrust.Accelerate = true;
		m_thrust.EnableBrake(false);
	}


	public void AttackTarget(float maxDistance)
	{
		maxDistance = 0;
		for(int i = 0; i < m_weaponSlots.Length;i++)
		{
			if(m_weapRange[i] > maxDistance)
				maxDistance = m_weapRange[i];
		}
		if(m_attackPos == Vector2.zero)
		{
			float xPos = Random.Range(-maxDistance, maxDistance);
			float yPos = Random.Range(-maxDistance, maxDistance);

			m_attackPos = new Vector2(xPos, yPos);
		}
		if(DistanceTo(m_target.position) > maxDistance)
		{
			FaceTarget((Vector2)m_target.position + m_attackPos);
			if(!m_obstacle)
				m_thrust.Accelerate = true;
		}
		else
		{
			m_thrust.Accelerate = false;
			FaceTarget(m_target.position);
			m_attackPos = Vector2.zero;
			for(int i = 0; i < m_weaponSlots.Length;i++)
			{
				if(AngleToTarget(m_target.position) < Mathf.Clamp(m_weapSpread[i], 10.0f, 90.0f) && CanShootTarget(m_target))
				{
					FireWeapon();
				}
			}
		}
	}

	public bool CheckAggro()
	{

		aggro = false;
		
		// if the current target is nonLethal and a lethal target comes in range
		if(m_target != null && nonLethalObj.Contains(m_target.gameObject) 
		   && (playerTarget || lethalObj.Count > 0))
		{
			m_target = null;
		}
		else if (m_target !=null)
		{
			aggro = true;
			return true;
		}

		if(nonLethalObj.Count > 0 || lethalObj.Count > 0 || playerTarget)
		{
			SelectTarget();
			aggro = true;
			return true;
		}

		foreach(GameObject g in squad)
		{
			if(g!= null && g.GetComponent<AIShipScript>().aggro && g != this.gameObject)
			{
				SelectTarget();
				return true;
			}
		}

		
		return false;

	}
	 

	// Follow the target, staying in between the max distance and min distance
	public void Chase(float maxDistance, float minDistance, Transform target)
	{
		FaceTarget(target.position);

		float distance = Vector2.Distance(target.position, transform.position);
		if(distance < minDistance)
		{
			Stop ();
		}
		else if(distance > maxDistance)
		{
			Go ();
		}
	}


	public float DistanceTo(Vector2 target)
	{
		return Vector2.Distance(transform.position, target);
	}

	// Fire weapon at the index, if no idex is provided fire all weapons
	public void FireWeapon(int index)
	{
		if(m_weaponSlots.Length > index)
		{
			m_weaponSlots[index].Fire();
		}
	}
	 
	public void FireWeapon()
	{
		for(int i = 0; i < m_weaponSlots.Length; i++)
		{

			m_weaponSlots[i].Fire();

		}
	}

	public void Wander()
	{
		m_wanderAngle += Random.Range(-0.05f, 0.05f);
		Vector2 wanderPos = 3.0f * new Vector2(Mathf.Cos(m_wanderAngle), Mathf.Sin(m_wanderAngle));
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
			if(g != null)
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
		else if(m_thrust.AccelPercent > 0.7f)
			m_thrust.AccelPercent = 0.7f;

	}

	public bool CanShootTarget(Transform targetTrans)
	{
		if(targetTrans == null)
			return false;

		float targetDist = Vector2.Distance(targetTrans.position, transform.position);
		if(targetDist > 20.0f)
			return false;
		
		// if there is something on top of the weapon, don't fire
		foreach(Collider2D col in Physics2D.OverlapPointAll(transform.position))
		{
			if(col.gameObject != this.gameObject)
				return false;
		}

		// if there is something in the way that's not a projectile, return false
		RaycastHit2D hit = Physics2D.Linecast(transform.position, targetTrans.position, LayerMask.NameToLayer("Projectiles"));
		if(hit.collider != null && hit.collider.gameObject != targetTrans.gameObject)
			return false;

		foreach(GameObject g in friendObj)
		{
			if(g != null)
			{
				// if there is a friend in front and within the weapon spread range, don't shoot
				if(Vector2.Dot(transform.up, g.transform.position - transform.position) > 0
				   && AngleToTarget(g.transform.position) < m_weapSpread[0])
				{
					return false;
				}
			}
		}


		return true;
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
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, GetComponent<Rigidbody2D>().velocity, 10.0f);

		foreach(RaycastHit2D h in hits)
		{
			if(h && (h.collider.gameObject.tag == "Asteroid" 
			           || h.collider.gameObject.tag == "Satellite"
			           || h.collider.gameObject.tag == "SAsteroid"))
			{
				m_obstacle = true;
				obstacleTrans = h.collider.gameObject.transform;
				return;
			}
		}
	}



	// return the angle between the direction the AI ship is facing
	// and the direction to the target's predicted position
	public float AngleToTarget(Vector2 targetPos)
	{
		Vector2 target = targetPos - (Vector2)transform.position;
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
	

		return predictPos;
	}

	void OnTriggerStay2D(Collider2D col)
	{

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Ship")
		{
			foreach(string s in enemies)
			{
				if(col.gameObject.name.Contains(s))
				{
					// figure out what kind of ship this is and change the appropriate variables
					switch(s)
					{
					case "Player Ship":
						playerTarget = true;
						break;
					case "CopShip":
					case "CriminalShip":
					case "CriminalLeader":
						lethalObj.Add(col.gameObject);
						break;
					default:
						nonLethalObj.Add(col.gameObject);
						break;
					}
					return;
				}

			}

			foreach(string s in friends)
			{
				if(col.gameObject.name.Contains(s))
				{
					friendObj.Add(col.gameObject);
					return;
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(PlayerShipScript.player.gameObject != null &&
		   col.gameObject == PlayerShipScript.player.gameObject)
		{
			playerTarget = false;
			return;
		}
		for(int i = 0; i < lethalObj.Count; i++)
		{
			if(col.gameObject == lethalObj[i])
			{
				lethalObj.Remove(col.gameObject);
			}
		}

		for(int i = 0; i < nonLethalObj.Count; i++)
		{
			if(col.gameObject == nonLethalObj[i])
			{
				nonLethalObj.Remove(col.gameObject);
			}
		}

		for(int i = 0; i < friendObj.Count; i++)
		{
			if(col.gameObject == friendObj[i])
			{
				friendObj.Remove(col.gameObject);
			}
		}
	}

	void OnDestroy()
	{
		squad.Remove (this.gameObject);
	}

	protected override void Die()
	{
		switch(GetComponent<ShipBehaviourScript>().behaviour)
		{
		case ShipBehaviourScript.Behaviour.Turret:
			GetComponent<SpriteRenderer>().sprite = destroyedSprite;
			GetComponent<CircleCollider2D>().enabled = false;
			m_weaponSlots[0].Weapon.GetComponent<SpriteRenderer>().enabled = false;
			Destroy(m_weaponSlots[0].Weapon);
			Instantiate(m_exploder, transform.position, Quaternion.identity);
			break;
		default:
			base.Die();
			break;
		}
	}
}
