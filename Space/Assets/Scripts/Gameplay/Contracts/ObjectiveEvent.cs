using UnityEngine;
using System.Collections;

public class ObjectiveEvent : MonoBehaviour {

	public enum ObjectiveType
	{
		GoTo,
		KillTarget,
		TurnInContract
	}

	public GameObject AISpawner;
	public GameObject spaceStation;

	private Contract objectiveContract;
	private GameObject target;
	private GameObject nextObjective;
	ObjectiveType type;


	//Look into enums for different objective types

	public Contract ObjectiveContract {
		get { return objectiveContract; }
		set { objectiveContract = value; }
	}

	public GameObject NextObjective{
		get{ return nextObjective;}
		set{ nextObjective = value;}
	}

	void OnEnable()
	{
		if(type == ObjectiveType.TurnInContract)
		{

			transform.position = spaceStation.transform.position;
			transform.parent = spaceStation.transform;
		}
	}

	public bool CheckIfNextObjective()
	{
		if(nextObjective == null)
		{
			return false;
		}

		return true;
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
			case ObjectiveType.TurnInContract:
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
					if(CheckIfNextObjective())
					{
						//Set the next objective to active and update the minimap
						nextObjective.SetActive(true);
						objectiveContract.SetUIMarker(nextObjective);
					}
					CompleteTask();
				}
				break;
		}
	}

	void OnTriggerEnter2D( Collider2D col )
	{
		if( col.tag == "Ship" && type != ObjectiveType.KillTarget)
		{
			//objectiveContract.completed = true;//Make boolean an array for multi-mission contracts
			CompleteTask();
		}
	}
}
