using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectivesUIController : MonoBehaviour {

	public CanvasGroup canvasGroup;

	public GameObject objectiveTextPrefab;

	public GameObject viewArea;

	public List<int> currentObjectives;
	public List<Button> currentButtons;

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
			int _objectiveInstanceID = objective.gameObject.GetInstanceID();

			Button objectiveBtn = objectiveUIObject.GetComponent<Button>();

			objectiveBtn.onClick.AddListener(()=>PlayerShipScript.player.ObjectiveMarker.GetComponent<UIMarker>().SetCurrentObjectiveSelected(_objectiveInstanceID));

			objectiveUIObject.transform.GetChild(0).GetComponent<Text>().text = objectiveText;

			currentObjectives.Add(_objectiveInstanceID);
			currentButtons.Add(objectiveBtn);
		}
	}

	public void SetButtonColor(int id, Vector4 color)
	{
		for(int i = 0; i < currentObjectives.Count; i++)
		{
			if(currentObjectives[i] == id)
			{
				currentButtons[i].GetComponent<Image>().color = color;
			}
		}
	}

	public void CompleteTask(int objectiveIndex)
	{
		if((objectiveIndex+1) < currentObjectives.Count)
		{
			SetButtonColor(currentObjectives[objectiveIndex+1],new Vector4(255f,255f,255f));
		}

		currentObjectives.RemoveAt (objectiveIndex);
		currentButtons.RemoveAt (objectiveIndex);
		Destroy (viewArea.transform.GetChild (objectiveIndex).gameObject);
		Debug.Log ("Task done");
	}
}
