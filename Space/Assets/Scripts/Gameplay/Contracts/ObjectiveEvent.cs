using UnityEngine;
using System.Collections;

public class ObjectiveEvent : MonoBehaviour {

	public enum ObjectiveType
	{
		GoTo,
		KillTarget
	}

	public GameObject AISpawner;

	private Contract objectiveContract;
	private GameObject target;
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
				GameObject spawner = (GameObject) GameObject.Instantiate(AISpawner,transform.position, Quaternion.identity);
 				target = spawner.GetComponent<AISpawnerScript>().SquadLeader;
				break;
		}
	}

	private void CompleteTask()
	{
		objectiveContract.CompleteContractObjective(this.gameObject);
		objectiveContract.Player.ObjectiveMarker.GetComponent<UIMarker>().removeTargetFromStack(this.gameObject);
		GameObject.Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		switch(type)
		{
			case ObjectiveType.KillTarget:
				if(target == null)
				{
					CompleteTask();	
				}
				break;
		}
	}

	void OnTriggerEnter2D( Collider2D col )
	{
		if( col.tag == "Ship" && type == ObjectiveType.GoTo )
		{
			//objectiveContract.completed = true;//Make boolean an array for multi-mission contracts
			CompleteTask();
		}
	}
}
