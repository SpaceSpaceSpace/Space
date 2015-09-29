using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Contract
{
	public bool completed;
	private Vector3 objectivePosition;
	private string description;
	private string targetImage;

	public Contract()
	{
		completed = false;
		objectivePosition = new Vector3 (Random.Range(-100,100), Random.Range(-100,100), 0);
		description = "Go here!";
		targetImage = "Image Directory";
	}

	public void CompleteContract()
	{
		completed = true;
	}

	public void SpawnContract(PlayerShipScript player)
	{
		GameObject contractObjective = (GameObject)GameObject.Instantiate (player.objectivePrefab, objectivePosition, Quaternion.identity);
		contractObjective.GetComponent<ObjectiveEvent> ().ObjectiveContract = this;
	}
}
