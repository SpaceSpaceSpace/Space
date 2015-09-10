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
		Patrol,
		Agressive,
		Defensive
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
			break;
		}
	}

	void Agressive()
	{

	}
}
