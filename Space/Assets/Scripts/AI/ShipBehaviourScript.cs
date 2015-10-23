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
		Leader,
		Cargo
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
	
		if(!PlayerShipScript.player.Alive)
			return;

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
			}
	}

	public void Civilian()
	{
		m_shipScript.Wander();
	}

	public void Grunt()
	{
		m_shipScript.Go ();
		// If the leader is alive and the player is less than 10 units away, move toward it
		if(m_shipScript.CheckAggro(15.0f))
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45.0f && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
			if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 15.0)
			{
				m_shipScript.AttackTarget(10.0f);
			}
			else
				m_shipScript.MoveToward(m_shipScript.Target);


		}
		// If the leader is alive, and the player is not near
		else
		{
			if(m_shipScript.DistanceTo(m_shipScript.objective.position) > 25.0f)
				m_shipScript.MoveToward(m_shipScript.objective);
			else
				m_shipScript.Flock();
			m_shipScript.aggro = false;

			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				m_shipScript.FaceTarget(m_shipScript.obstacleTrans.position);
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 10 && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
		}
	}

	public void Leader()
	{
		// if the player is not near
		if(m_shipScript.CheckAggro(15.0f))
		{
			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 45.0f && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
			if(m_shipScript.DistanceTo(m_shipScript.Target.position) < 15.0)
			{
				m_shipScript.AttackTarget(10.0f);
			}
			else
				m_shipScript.MoveToward(m_shipScript.Target);
		}
		else if(m_shipScript.DistanceTo(m_shipScript.Target.position) >10.0f)
		{
			if(m_shipScript.DistanceTo(m_shipScript.objective.position) > 25.0f)
				m_shipScript.MoveToward(m_shipScript.objective);
			else
				m_shipScript.Flock();
			m_shipScript.aggro = false;

			if(m_shipScript.Obstacle)
			{
				m_shipScript.Stop();
				m_shipScript.FaceTarget(m_shipScript.obstacleTrans.position);
				if(m_shipScript.AngleToTarget(m_shipScript.obstacleTrans.position) < 10 && m_shipScript.CanSeeTarget(m_shipScript.obstacleTrans))
				{
					m_shipScript.FireWeapon();
				}
			}
		}

	}

	public void Cargo()
	{
		if(m_shipScript.Obstacle)
			m_shipScript.Stop();
		else
		{
			int index = m_shipScript.squad.IndexOf(this.gameObject);
			if(index != 0)
				m_shipScript.Chase(5.0f, 4.0f, m_shipScript.squad[index - 1].transform);
			else
			{
				if(m_shipScript.DistanceTo(m_shipScript.squad[1].transform.position) < 5.0f)
					m_shipScript.MoveForward();
			}
		}

	}
}
