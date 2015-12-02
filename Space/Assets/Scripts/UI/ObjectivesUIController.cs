using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectivesUIController : MonoBehaviour {

	public CanvasGroup canvasGroup;

	public GameObject objectiveTextPrefab;

	public GameObject viewArea;

	public GameObject[] objectivesList;

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

			objectiveUIObject.transform.GetChild(0).GetComponent<Text>().text = objectiveText;

			Debug.Log(objective.Objective.objectiveType);
		}
	}
}
