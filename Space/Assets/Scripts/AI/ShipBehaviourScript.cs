using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AIShipScript))]

// This script contains methods which define how an AI ship will behave
// based on its behaviour value 
public class ShipBehaviourScript : MonoBehaviour {

	///
	/// Public
	///
	// The difference Behaviours a ship can have
	public enum Behaviour
	{
		Asleep,
		Civilian,
		Grunt,
		Leader
	}

	public Behaviour behaviour; // to be defined in the inspector

	/// 
	/// Private
	/// 
	private AIShipScript m_shipScript;
	private float chaseTime; // how long will an enemy chase

	// Use this for initialization
	void Start () {
		m_shipScript = GetComponent<AIShipScript>();
		chaseTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!PlayerShipScript.player.Alive)
			return;

		DoNormalStuff();
		if(m_shipScript.Obstacle)
			m_shipScript.FireWeapon(0);

		chaseTime += Time.deltaTime;
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
			}
	}

	public void Civilian()
	{
		m_shipScript.Wander();
	}

	public void Grunt()
	{
		// If the leader is dead
		if(m_shipScript.leader == null)
		{
			// and the player is not close, wander
			if(m_shipScript.DistanceTo(m_shipScript.Target.position) > 20.0f)
				m_shipScript.Wander();
			// if the player is close, run away
			else
			{
				m_shipScript.MoveAwayFrom(m_shipScript.player);
			}

		}
		else if(chaseTime > 3.0f)
		{

			chaseTime+=Time.deltaTime;

			if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 10.0f)
				m_shipScript.MoveAwayFrom(m_shipScript.Target);
			else
				m_shipScript.MoveToward(m_shipScript.Target);

			if(chaseTime > 0.0f)
				chaseTime = 0.0f;

		}
		// If the leader is alive and the player is less than 10 units away, move toward it
		else if(m_shipScript.CheckAggro())
		{

			if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 5.0)
			{
				if(m_shipScript.AngleToTarget() < 10.0f && m_shipScript.CanSeeTarget())
				{
					m_shipScript.FireWeapon(0);
				}
				m_shipScript.ChaseTarget(5.0f, 3.0f);
				chaseTime+=Time.deltaTime;
			}
			else
				m_shipScript.MoveToward(m_shipScript.Target);


		}
		// If the leader is alive, and the player is not near
		else
		{
			m_shipScript.aggro = false;
			m_shipScript.Flock();
		}
	}

	public void Leader()
	{
		// if the player is not near
		if(m_shipScript.CheckAggro())
		{
			if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 5.0)
			{
				if(m_shipScript.AngleToTarget() < 10.0f && m_shipScript.CanSeeTarget())
				{
					m_shipScript.FireWeapon(0);
				}
				m_shipScript.ChaseTarget(5.0f, 3.0f);
			}
			else
				m_shipScript.MoveToward(m_shipScript.Target);
		}
		else if(m_shipScript.DistanceTo(m_shipScript.Target.position) >10.0f)
		{
			m_shipScript.aggro = false;
			m_shipScript.Flock();
		}

	}



}
