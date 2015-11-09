using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Contract
{
	public bool completed;
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
        Objective objective1 = new ObjectiveKillTarget();
        Objective objective2 = new ObjectiveTurnInContract();

        GameObject contractObjective1 = (GameObject)GameObject.Instantiate(objectivePrefab, objective1.Position, Quaternion.identity);
        ObjectiveEvent contractObjectiveEvent1 = contractObjective1.GetComponent<ObjectiveEvent>();
        contractObjectiveEvent1.ObjectiveContract = this;
        contractObjectiveEvent1.ToComplete = objective1;
        SetUIMarker (contractObjective1);

		GameObject contractObjective2 = (GameObject)GameObject.Instantiate (objectivePrefab, objective2.Position, Quaternion.identity);
        ObjectiveEvent contractObjectiveEvent2 = contractObjective2.GetComponent<ObjectiveEvent>();
        contractObjectiveEvent2.ObjectiveContract = this;
        contractObjectiveEvent2.ToComplete = objective2;
        contractObjective2.SetActive (false);

        contractObjectiveEvent1.NextObjective = contractObjective2;

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
