using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Contract
{
	public bool completed;
	private Vector3 objectivePosition;
	private string description;
	private string targetImage;
	private List<GameObject> contractObjectives;
	private PlayerShipScript player;

	public Contract()
	{
		contractObjectives = new List<GameObject> ();
		//contractObjectives.Add(
		completed = false;
		objectivePosition = new Vector3 (Random.Range(-100,100), Random.Range(-100,100), 0);
		description = "Go here!";
		targetImage = "Image Directory";
		player = GameObject.Find ("Player Ship").GetComponent<PlayerShipScript>();
	}

	public PlayerShipScript Player
	{
		get{return player;}
	}

	public void CompleteContractObjective(GameObject completedObjective)
	{
		contractObjectives.Remove (completedObjective); //UPDATE UI

		if(contractObjectives.Count == 0)
		{
			completed = true;
		}
	}

	//Eventually will spawn objectives based off contract
	public void SpawnContract(PlayerShipScript player)
	{
		GameObject contractObjective1 = (GameObject)GameObject.Instantiate (player.objectivePrefab, objectivePosition, Quaternion.identity);
		contractObjective1.GetComponent<ObjectiveEvent> ().ObjectiveContract = this;
		contractObjective1.GetComponent<ObjectiveEvent> ().init (ObjectiveEvent.ObjectiveType.KillTarget);
		SetUIMarker (contractObjective1);

		GameObject contractObjective2 = (GameObject)GameObject.Instantiate (player.objectivePrefab, objectivePosition, Quaternion.identity);
		contractObjective2.GetComponent<ObjectiveEvent> ().ObjectiveContract = this;
		contractObjective2.GetComponent<ObjectiveEvent> ().init (ObjectiveEvent.ObjectiveType.TurnInContract);
		contractObjective2.SetActive (false);

		contractObjective1.GetComponent<ObjectiveEvent> ().NextObjective = contractObjective2;

		contractObjectives.Add (contractObjective1);
		contractObjectives.Add (contractObjective2);
	}

	public void SetUIMarker(GameObject contractObjective)
	{
		GameObject oMarker =  player.ObjectiveMarker;

		if(!oMarker.activeSelf)
		{
			oMarker.SetActive(true);
		}

		UIMarker markerScript = oMarker.GetComponent<UIMarker> ();

		markerScript.AddToTargetStack (contractObjective);
	}
}
