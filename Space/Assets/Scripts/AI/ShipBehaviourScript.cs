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
		Agressive,
		Defensive,
		Flee,
		Wander
	}

	public Behaviour behaviour; // to be defined in the inspector

	/// 
	/// Private
	/// 
	private AIShipScript m_shipScript;

	// Use this for initialization
	void Start () {
		m_shipScript = GetComponent<AIShipScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
		switch (behaviour)
		{
		case Behaviour.Agressive:
			Agressive ();
			break;
		case Behaviour.Wander:
			Wander();
			break;
		case Behaviour.Defensive:
			Defensive();
			break;
		}
	}

	public void Civilian()
	{

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
				m_shipScript.MoveAwayFrom(m_shipScript.player);
		}
		else if(m_shipScript.DistanceTo(m_shipScript.Target.position) > 10.0f)
			m_shipScript.MoveToward(m_shipScript.leader.transform);
		if(m_shipScript.CanSeeTarget())
		{
			if(m_shipScript.DistanceTo(m_shipScript.Target.position) > 5.0)
				m_shipScript.MoveToward(m_shipScript.Target);
			else
			{
				m_shipScript.FireWeapon(0);
				m_shipScript.ChaseTarget(5.0f, 3.0f);
			}
		}
	}
	// Agressive enemies will contantly move toward the target, 
	public void Agressive()
	{
		//if(m_shipScript.DistanceToTarget() > 5.0f)
		//	m_shipScript.MoveTowardTarget();
		//else
		//{
		//	m_shipScript.FireWeapon(0);
		//	m_shipScript.ChaseTarget(5.0f, 3.0f);
		//}
	}

	// Wandering enemies will do just that
	public void Wander()
	{
		m_shipScript.Wander();
	}

	public void Defensive()
	{
		//if(m_shipScript.DistanceToTarget() < 6.0f)
		//{
		//	m_shipScript.ResetPassSide();
		//	m_shipScript.MoveAwayFromTarget();
		//}
		//else
		//	m_shipScript.PassByTarget(10.0f);
	}


}
