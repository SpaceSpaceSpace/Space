using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Contract
{
	public bool completed;
	private string description;
	private string targetImagePath;
	private Image targetImage;
	private string targetShipImagePath;
	private Image targetShipImage;
	private string name;
	private string title;
	private string reward;
	private List<Objective> contractObjectives;
    private List<ObjectiveEvent> objectiveEvents;
    private GameObject objectivePrefab;

	public Contract()
	{
		contractObjectives = new List<Objective> ();
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
		contractObjectives = new List<Objective> ();

        contractObjectives.Add(new ObjectiveKillTarget());
        contractObjectives.Add(new ObjectiveTurnInContract());

        completed = false;
		//targetImagePath = "Image Directory";
		//targetShipImagePath = "ShipImage Directory";
		name = p_Name;
		title = p_Title;
		description = p_Description;
		reward = p_Reward;
        objectivePrefab = Resources.Load("Objective") as GameObject;
    }

    public Contract(string p_Name, string p_Description, string p_Title, string p_ImagePath, string p_ShipImagePath, Objective[] p_Objectives)
    {
        contractObjectives = p_Objectives.ToList();
        completed = false;
        targetImagePath = p_ImagePath;
        targetShipImagePath = p_ShipImagePath;
        name = p_Name;
        title = p_Title;
        description = p_Description;
        reward = "";
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

	public void CompleteContractObjective(Objective completedObjective)
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
        objectiveEvents = new List<ObjectiveEvent>();

        for (int i = 0; i < contractObjectives.Count; i++)
        {
            Objective objective = contractObjectives[i];

            GameObject contractObjectiveObject = (GameObject)GameObject.Instantiate(objectivePrefab, objective.Position, Quaternion.identity);
            ObjectiveEvent contractObjectiveEvent = contractObjectiveObject.GetComponent<ObjectiveEvent>();
            contractObjectiveEvent.ObjectiveContract = this;
            contractObjectiveEvent.ToComplete = objective;

            objectiveEvents.Add(contractObjectiveEvent);

            if (i == 0)
            {
                SetUIMarker(contractObjectiveObject);
            }
            else
            {
                objectiveEvents[i - 1].NextObjective = contractObjectiveObject;
                contractObjectiveObject.SetActive(false);
            }  
        }
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
