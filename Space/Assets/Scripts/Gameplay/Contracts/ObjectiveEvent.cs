using UnityEngine;
using System.Collections;

public class ObjectiveEvent : MonoBehaviour {

	public enum ObjectiveType
	{
		GoTo,
		KillTarget
	}

	public AISpawnerScript AISpawner;

	private Contract objectiveContract;
	ObjectiveType type;

	//Look into enums for different objective types

	public Contract ObjectiveContract {
		get { return objectiveContract; }
		set { objectiveContract = value; }
	}

	public void init(ObjectiveType p_Type)
	{
		type = p_Type;

		//Spawn Correct Objective
		switch(type)
		{
			case ObjectiveType.GoTo:
				break;
			case ObjectiveType.KillTarget:
				break;
				
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D( Collider2D col )
	{
		if( col.tag == "Ship" )
		{
			//objectiveContract.completed = true;//Make boolean an array for multi-mission contracts
			objectiveContract.CompleteContractObjective(this.gameObject);
			objectiveContract.Player.ObjectiveMarker.GetComponent<UIMarker>().removeTargetFromStack(this.gameObject);
			GameObject.Destroy(this.gameObject);
		}
	}
}
