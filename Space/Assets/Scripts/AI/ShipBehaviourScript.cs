using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AIShipScript))]

// This script contains methods which define how an AI ship will behave
// based on its behaviour value 
public class ShipBehaviourScript : MonoBehaviour {

	///
	/// Private
	///
	/// Public
	///
	// The difference Behaviours a ship can have
	public enum Behaviour
	{
		Asleep,
		Civilian,
		Grunt,
		Leader,
		Cargo,
		Cop,
		Rescue,
		Turret
	}

	public Behaviour behaviour; // to be defined in the inspector

	/// 
	/// Private
	/// 
	private AIShipScript m_shipScript;

	// Use this for initialization
	void Start () {
		m_shipScript = GetComponent<AIShipScript>();
		switch (behaviour) {
		case Behaviour.Civilian:
		case Behaviour.Cargo:
		case Behaviour.Cop:
		case Behaviour.Rescue:
			m_shipScript.enemies = new string[2]{ "CriminalShip", "CriminalLeader" };
			m_shipScript.friends = new string[3]{"Player Ship", "CargoShip", "CopShip"};
			break;
		case Behaviour.Grunt:
		case Behaviour.Leader:
		case Behaviour.Turret:
			m_shipScript.enemies = new string[3]{ "Player Ship", "CargoShip", "CopShip" };
			m_shipScript.friends = new string[2]{ "CriminalShip", "CriminalLeader" };
			break;
		}

		// For special objects which won't be spawned through AISpawners 
		switch (GetComponent<ShipBehaviourScript>().behaviour)
		{
		case ShipBehaviourScript.Behaviour.Turret:
			AIShipScript ss = GetComponent<AIShipScript>();
			
			ss.InitWeapons();
			WeaponScript.WeaponType weapon = (WeaponScript.WeaponType) Random.Range(0, (int)WeaponScript.WeaponType.SCATTER_SHOT + 1);
			ss.WeaponSlots[ 0 ].SetWeapon( Instantiate( GameMaster.WeaponMngr.GetWeaponPrefab( weapon ) ) );
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!PlayerShipScript.player.Alive)
			return;

		// if you reach the edge of the sector, return, unless it's a cargo ship
		if(!(behaviour == Behaviour.Cargo) && (Mathf.Abs(transform.position.x) > 300.0f || Mathf.Abs(transform.position.y) > 300.0f))
		{
			m_shipScript.FaceTarget(Vector2.zero);
			m_shipScript.MoveForward();
		}
		else
			DoNormalStuff();
	}

	public void DoNormalStuff() // to be fxed later
	{
		switch (behaviour) {
		case Behaviour.Civilian:
			Civilian ();
			break;
		case Behaviour.Grunt:
			Grunt ();
			break;
		case Behaviour.Leader:
			Leader ();
			break;
		case Behaviour.Cargo:
			Cargo();
			break;
		case Behaviour.Cop:
			Cop ();
			break;
		case Behaviour.Rescue:
			Rescue();
			break;
		case Behaviour.Turret:
			Turret();
			break;
		}
	}

	public void Civilian()
	{
		m_shipScript.Wander();
	}

	public void Grunt()
	{
		m_shipScript.Go ();
		// If the player is less than 15 units away, move toward it
		if(m_shipScript.CheckAggro())
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45.0f && m_shipScript.CanShootTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
			if(m_shipScript.Target != null)
			{
				if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 20.0)
				{
					m_shipScript.AttackTarget(7.5f);
				}
				else
					m_shipScript.MoveToward(m_shipScript.Target);
			}
		}
		// If the player is not near
		else
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				m_shipScript.FaceTarget(m_shipScript.obstacleTrans.position);
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 15.0f && m_shipScript.CanShootTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
			else if(m_shipScript.objective != null && m_shipScript.DistanceTo(m_shipScript.ObjectiveStartPos) > 25.0f)
			{
				if(m_shipScript.objective != null)
					m_shipScript.MoveToward(m_shipScript.objective);
			}
			else
				m_shipScript.Flock();
			m_shipScript.aggro = false;
		}
	}

	public void Leader()
	{
		// if the player is near
		if(m_shipScript.CheckAggro())
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45.0f && m_shipScript.CanShootTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
			if(m_shipScript.Target != null)
			{
				if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 20.0)
				{
					m_shipScript.AttackTarget(10.0f);
				}
				else
					m_shipScript.MoveToward(m_shipScript.Target);
			}
		}
		else
		{
			if(m_shipScript.objective != null && m_shipScript.DistanceTo(m_shipScript.ObjectiveStartPos) > 25.0f)
			{
				if(m_shipScript.objective != null)
					m_shipScript.MoveToward(m_shipScript.objective);
			}	
			else
				m_shipScript.Flock();
			m_shipScript.aggro = false;

			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				m_shipScript.FaceTarget(m_shipScript.obstacleTrans.position);
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45 && m_shipScript.CanShootTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
		}

	}

	public void Cop()
	{
		m_shipScript.Go ();
		// If the player is less than 15 units away, move toward it
		if(m_shipScript.CheckAggro())
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45.0f && m_shipScript.CanShootTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
			if(m_shipScript.Target != null)
			{
				if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 15.0)
				{
					m_shipScript.AttackTarget(7.5f);
				}
				else
					m_shipScript.MoveToward(m_shipScript.Target);
			}
			
			
		}
		// If the player is not near
		else
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				m_shipScript.FaceTarget(m_shipScript.obstacleTrans.position);
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 15.0f && m_shipScript.CanShootTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
			else if(m_shipScript.objective != null && m_shipScript.DistanceTo(m_shipScript.ObjectiveStartPos) > 25.0f)
			{
				if(m_shipScript.objective != null)
					m_shipScript.MoveToward(m_shipScript.objective);
			}
			else
				m_shipScript.Flock();
			m_shipScript.aggro = false;
		}
	}

	public void Cargo()
	{
		if(m_shipScript.Obstacle)
			m_shipScript.Stop();
		else
		{
			if(m_shipScript.objective != null)
				m_shipScript.FaceTarget(m_shipScript.objective.position);
			int index = m_shipScript.squad.IndexOf(this.gameObject);
			if(index != 0)
				m_shipScript.Chase(5.0f, 4.0f, m_shipScript.squad[index - 1].transform);
			else
			{

				if((m_shipScript.squad.Count == 1 || m_shipScript.DistanceTo(m_shipScript.squad[1].transform.position) < 5.0f) && 
				    m_shipScript.DistanceTo(PlayerShipScript.player.transform.position) < 15.0f)
				{
					m_shipScript.Go();
				}
				else
					m_shipScript.Stop();
			}
		}

		if(Mathf.Abs(transform.position.x) > 400 || Mathf.Abs(transform.position.y) > 400)
			Destroy(this.gameObject);
	}

	public void Rescue()
	{
		if(m_shipScript.DistanceTo(PlayerShipScript.player.transform.position) < 10.0f)
		{
			m_shipScript.Chase(5.0f, 4.0f, PlayerShipScript.player.transform);
		}
	}

	public void Turret()
	{
		if(m_shipScript.WeaponSlots[0].Weapon != null)
		{
			// If the player is less than 15 units away, move toward it
			if(m_shipScript.CheckAggro())
			{
				if(m_shipScript.Target != null)
				{
					m_shipScript.TurnWeapon();
					Vector2 weaponRot = m_shipScript.WeaponSlots[0].Weapon.transform.up;
					if(Vector2.Angle(weaponRot, m_shipScript.Target.position - transform.position) < 45.0f && m_shipScript.CanShootTarget(m_shipScript.Target))
					{
						m_shipScript.FireWeapon();
					}
				}
			}
		}
	}
}
