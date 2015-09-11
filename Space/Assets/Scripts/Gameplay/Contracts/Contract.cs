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

	public Contract()
	{
		contractObjectives = new List<GameObject> ();
		completed = false;
		objectivePosition = new Vector3 (Random.Range(-100,100), Random.Range(-100,100), 0);
		description = "Go here!";
		targetImage = "Image Directory";
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
		GameObject contractObjective = (GameObject)GameObject.Instantiate (player.objectivePrefab, objectivePosition, Quaternion.identity);
		contractObjective.GetComponent<ObjectiveEvent> ().ObjectiveContract = this;
		SetUIMarker (contractObjective);
		contractObjectives.Add (contractObjective);
		SetUIMarker (contractObjective);
	}

	public void SetUIMarker(GameObject contractObjective)
	{
		GameObject oMarker =  PlayerShipScript.player.ObjectiveMarker;

		if(!oMarker.activeSelf)
		{
			oMarker.SetActive(true);
		}

		UIMarker markerScript = oMarker.GetComponent<UIMarker> ();

		markerScript.AddToTargetStack (contractObjective);
	}
}
