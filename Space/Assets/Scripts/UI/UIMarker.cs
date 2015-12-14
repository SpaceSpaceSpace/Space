using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMarker : MonoBehaviour {

	public List<GameObject> targetStack = new List<GameObject>();

	private int currentObjectiveSelected;
	public int CurrentObjectiveSelected
	{
		get { return currentObjectiveSelected; } 
		set { currentObjectiveSelected = value; }
	}

	// Use this for initialization
	void Start () {
	}

	public void AddToTargetStack(GameObject newTarget)
	{
		targetStack.Add (newTarget);
		currentObjectiveSelected = targetStack.Count - 1;
		//targetStack.Insert (0, newTarget);
	}

	public void removeTargetFromStack(GameObject completedTarget)
	{
		targetStack.Remove (completedTarget);

		if(targetStack.Count == 0)
		{
			gameObject.SetActive(false);
		}

		currentObjectiveSelected = targetStack.Count - 1;
	}

	public void SetCurrentObjectiveSelected(int targetInstanceID)
	{
		for(int i = 0; i < targetStack.Count; i++)
		{
			if(targetStack[i].GetInstanceID() == targetInstanceID)
			{
				UI_Manager.instance.objectivesUIController.SetButtonColor(targetInstanceID, new Vector4(255f,255f,255f,255f));
				currentObjectiveSelected = i;
				return;
			}
			else
			{
				UI_Manager.instance.objectivesUIController.SetButtonColor(targetInstanceID, new Vector4(255f,0f,0f,255f));
			}
		}
	}

	// Update is called once per frame
	void Update () {

        if (targetStack.Count == 0)
        {
            gameObject.SetActive(false);
        }

		transform.localPosition = Vector3.zero;
		Vector3 direction =  targetStack[currentObjectiveSelected].transform.position - transform.parent.transform.position;
		//Vector3 heading = targetStack [targetStack.Count - 1].transform.position + direction;

		Vector3 newMarkerPos = direction.normalized;
		newMarkerPos *= 4.0f;

		//transform.localPosition = newMarkerPos;
		transform.position = newMarkerPos + transform.parent.transform.position;

		//float angle = Vector3.Angle (Vector3.right, direction);

		float angle = Mathf.Atan2 (direction.y, direction.x);

		angle *= Mathf.Rad2Deg;

		transform.eulerAngles = new Vector3 (0, 0, angle+90.0f);

		Debug.DrawLine (targetStack[currentObjectiveSelected].transform.position-direction,targetStack[targetStack.Count - 1].transform.position,Color.blue);

		//Debug.DrawLine (targetStack [targetStack.Count - 1].transform.position,transform.parent.position, Color.red);
	}
}
