using UnityEngine;
using System;
using System.Collections.Generic;

public class ObjectiveEvent : MonoBehaviour
{
    private Contract objectiveContract;
    private GameObject target;
    private GameObject nextObjective;
    Objective objective;
    Type objectiveType;

    //Look into enums for different objective types

    public Contract ObjectiveContract
    {
        get { return objectiveContract; }
        set { objectiveContract = value; }
    }

    public Objective ToComplete
    {
        set { objective = value; }
    }

    public GameObject NextObjective
    {
        get { return nextObjective; }
        set { nextObjective = value; }
    }

    public bool CheckIfNextObjective()
    {
        if (nextObjective == null)
        {
            return false;
        }

        return true;
    }

	public Objective Objective
	{
		get { return objective; }
	}

    void Start()
    {
        objective.Position = transform.position;

        objectiveType = objective.GetType();

        objective.SetupObjective(gameObject);
    }

    private void CompleteTask()
    {
        //Can't complete unless objective conditions are met
        if (!objective.Completed)
            return;

        if (CheckIfNextObjective())
        {
            //Set the next objective to active and update the minimap
            nextObjective.SetActive(true);
            objectiveContract.SetUIMarker(nextObjective);
        }

        if (PlayerShipScript.player)
        {
            ObjectiveEvent objectiveEvent = gameObject.GetComponent<ObjectiveEvent>();
            objectiveContract.CompleteContractObjective(objectiveEvent.objective);
            PlayerShipScript.player.ObjectiveMarker.GetComponent<UIMarker>().removeTargetFromStack(gameObject);
        }

		List<ObjectiveEvent> events = UI_Manager.instance.objectivesUIController.currentObjectives;
		
		for(int i = 0; i < events.Count; i++)
		{
			if(events[i].gameObject == this.gameObject)
			{
				UI_Manager.instance.objectivesUIController.CompleteTask(i);
				break;
			}
		}

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        objective.ObjectiveUpdate();

        transform.position = objective.Position;

        if (objective.Completed)
            CompleteTask();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        objective.HitObjective(col);
    }
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}