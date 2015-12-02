using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Contract
{
	public bool completed;

    public string TargetName {get{ return name; } }
    public string Title { get { return title; } }
    public string Description { get { return description; } }
    public Sprite TargetImage { get { return targetImage; } }
    public Sprite TargetShipImage { get { return targetShipImage; } }
    public List<Objective> Objectives { get { return contractObjectives; } }

    private string name;
    private string title;
    private string description;
	private Sprite targetImage;
	private Sprite targetShipImage;
	private string reward;
	private List<Objective> contractObjectives;
    private List<ObjectiveEvent> objectiveEvents;
    private GameObject objectivePrefab;

	public Contract()
	{
		contractObjectives = new List<Objective> ();
		completed = false;
		description = "Go here!";
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
        targetImage = Resources.Load<Sprite>(p_ImagePath);
        targetShipImage = Resources.Load<Sprite>(p_ShipImagePath);
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

	public void CompleteContractObjective(Objective completedObjective)
	{
		contractObjectives.Remove (completedObjective);

		if(contractObjectives.Count == 0)
		{
			completed = true;
		}
	}
	
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

		//Populate objective UI
		UI_Manager.instance.PopulateObjectiveUI (objectiveEvents);
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
