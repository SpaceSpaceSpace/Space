using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectivesUIController : MonoBehaviour {

	public CanvasGroup canvasGroup;

	public GameObject objectiveTextPrefab;

	public GameObject viewArea;

	public List<ObjectiveEvent> currentObjectives;

	public void PopulateUIObjectives(List<ObjectiveEvent> objectiveEvents)
	{
		foreach(ObjectiveEvent objective in objectiveEvents)
		{
			string objectiveText = "Default";

			switch(objective.Objective.objectiveType)
			{
				case "EscortCargo": objectiveText = "Escort cargo ship"; break;
				case "TurnInContract": objectiveText = "Return contract to space station"; break;
				case "KillTarget": objectiveText = "Kill target ship"; break;
			}

			GameObject objectiveUIObject = Instantiate(objectiveTextPrefab) as GameObject;
			objectiveUIObject.transform.SetParent(viewArea.transform,false);
			GameObject _objective = objective.gameObject;

			Button objectiveBtn = objectiveUIObject.GetComponent<Button>();

			objectiveBtn.onClick.AddListener(()=>PlayerShipScript.player.ObjectiveMarker.GetComponent<UIMarker>().SetCurrentObjectiveSelected(_objective));

			objectiveUIObject.transform.GetChild(0).GetComponent<Text>().text = objectiveText;
		}

		currentObjectives = objectiveEvents;
	}

	public void CompleteTask(int objectiveIndex)
	{
		currentObjectives.RemoveAt (objectiveIndex);
		Destroy (viewArea.transform.GetChild (objectiveIndex).gameObject);
		Debug.Log ("Task done");
	}
}
