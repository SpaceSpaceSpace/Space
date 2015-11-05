using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Contract
{
	public bool completed;
	public Vector3 objectivePosition;
	private string description;
	//private string targetImagePath;
	//private Image targetImage;
	private string targetShipImagePath;
	private Image targetShipImage;
	private string name;
	private string title;
	private string reward;
	private List<GameObject> contractObjectives;
    private GameObject objectivePrefab;

	public Contract()
	{
		contractObjectives = new List<GameObject> ();
		completed = false;
		objectivePosition = new Vector3 (Random.Range(-100,100), Random.Range(-25,25), 0);
		description = "Go here!";
		//targetImagePath = "Image Directory";
		//targetShipImagePath = "ShipImage Directory";
		name = "Unknown";
		title = "Unknown Title";
		reward = "0 Space Dollars";
        objectivePrefab = Resources.Load("Objective") as GameObject;
	}

	public Contract(string p_Name, string p_Description, string p_Title, string p_Reward)
	{
		contractObjectives = new List<GameObject> ();
		completed = false;
		objectivePosition = new Vector3 (Random.Range(-100,100), Random.Range(-100,100), 0);
		//targetImagePath = "Image Directory";
		//targetShipImagePath = "ShipImage Directory";
		name = p_Name;
		title = p_Title;
		description = p_Description;
		reward = p_Reward;
        objectivePrefab = Resources.Load("Objective") as GameObject;
    }

	public string Name
	{
		get{ return name;}
	}

	public Dictionary<string,string> GetContractDetails()
	{
		Dictionary<string,string> contractDetails = new Dictionary<string, string> ();

		contractDetails.Add ("Name", name);
		contractDetails.Add ("Title", title);
		contractDetails.Add ("Reward", reward);
		contractDetails.Add ("Description", description);

		return contractDetails;
	}

	public void CompleteContractObjective(GameObject completedObjective)
	{
		contractObjectives.Remove (completedObjective);

		if(contractObjectives.Count == 0)
		{
			completed = true;
		}
	}

	//Eventually will spawn objectives based off contract
	public void SpawnContract()
	{
		GameObject contractObjective1 = (GameObject)GameObject.Instantiate (objectivePrefab, objectivePosition, Quaternion.identity);
		contractObjective1.GetComponent<ObjectiveEvent> ().ObjectiveContract = this;
		contractObjective1.GetComponent<ObjectiveEvent> ().init (ObjectiveType.KillTarget);
		SetUIMarker (contractObjective1);

		GameObject contractObjective2 = (GameObject)GameObject.Instantiate (objectivePrefab, objectivePosition, Quaternion.identity);
		contractObjective2.GetComponent<ObjectiveEvent> ().ObjectiveContract = this;
		contractObjective2.GetComponent<ObjectiveEvent> ().init (ObjectiveType.TurnInContract);
		contractObjective2.SetActive (false);

		contractObjective1.GetComponent<ObjectiveEvent> ().NextObjective = contractObjective2;

		contractObjectives.Add (contractObjective1);
		contractObjectives.Add (contractObjective2);
	}

	public void SetUIMarker(GameObject contractObjective)
	{
		GameObject oMarker = PlayerShipScript.player.ObjectiveMarker;

		if(oMarker != null && !oMarker.activeSelf)
		{
			oMarker.SetActive(true);
		}

		UIMarker markerScript = oMarker.GetComponent<UIMarker> ();

		markerScript.AddToTargetStack (contractObjective);
	}
}
