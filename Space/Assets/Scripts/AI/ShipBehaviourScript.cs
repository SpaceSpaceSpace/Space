﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AIShipScript))]

// This script contains methods which define how an AI ship will behave
// based on its behaviour value 
public class ShipBehaviourScript : MonoBehaviour {

	///
	/// Private
	///
	private string[] enemies;
	private string[] friends;
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
		Rescue
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
			enemies = new string[2]{ "CriminalShip", "CriminalLeader" };
			friends = new string[3]{"Player Ship", "CargoShip", "CopShip"};
			break;
		case Behaviour.Grunt:
		case Behaviour.Leader:
			enemies = new string[3]{ "Player Ship", "CargoShip", "CopShip" };
			friends = new string[2]{ "CriminalShip", "CriminalLeader" };
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!PlayerShipScript.player.Alive)
			return;

		// if you reach the edge of the sector, return, unless it's a cargo ship
		if(!(behaviour == Behaviour.Cargo) && (Mathf.Abs(transform.position.x) > 350.0f || Mathf.Abs(transform.position.y) > 350.0f))
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
		if(m_shipScript.CheckAggro(20.0f, enemies))
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45.0f && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans, friends))
				{
					m_shipScript.FireWeapon();
				}
			}
			if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 20.0)
			{
				m_shipScript.AttackTarget(7.5f, friends);
			}
			else
				m_shipScript.MoveToward(m_shipScript.Target);
		}
		// If the player is not near
		else
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				m_shipScript.FaceTarget(m_shipScript.obstacleTrans.position);
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 15.0f && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans, friends))
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
		if(m_shipScript.CheckAggro(20.0f, enemies))
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45.0f && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans,friends))
				{
					m_shipScript.FireWeapon();
				}
			}
			if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 20.0)
			{
				m_shipScript.AttackTarget(10.0f, friends);
			}
			else
				m_shipScript.MoveToward(m_shipScript.Target);
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
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45 && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans, friends))
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
		if(m_shipScript.CheckAggro(20.0f, enemies))
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45.0f && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans, friends))
				{
					m_shipScript.FireWeapon();
				}
			}
			if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 15.0)
			{
				m_shipScript.AttackTarget(7.5f, friends);
			}
			else
				m_shipScript.MoveToward(m_shipScript.Target);
			
			
		}
		// If the player is not near
		else
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				m_shipScript.FaceTarget(m_shipScript.obstacleTrans.position);
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 15.0f && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans, friends))
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
				    m_shipScript.DistanceTo(PlayerShipScript.player.transform.position) < 17.5f)
				{
					m_shipScript.MoveForward();
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
		if(m_shipScript.CheckAggro(15.0f, new string[1] {"Player Ship"}))
		{
			m_shipScript.Chase(5.0f, 4.0f, PlayerShipScript.player.transform);
		}
	}
}
