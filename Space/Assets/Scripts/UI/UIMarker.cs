using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMarker : MonoBehaviour {

	public List<GameObject> targetStack;

	// Use this for initialization
	void Start () {
	
	}

	public void AddToTargetStack(GameObject newTarget)
	{
		targetStack.Add (newTarget);
	}

	public void removeTargetFromStack(GameObject completedTarget)
	{
		targetStack.Remove (completedTarget);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = Vector3.zero;
		Vector3 direction =  targetStack[targetStack.Count - 1].transform.position - transform.parent.transform.position;
		//Vector3 heading = targetStack [targetStack.Count - 1].transform.position + direction;

		Vector3 newMarkerPos = direction.normalized;
		newMarkerPos *= 1.0f;

		transform.localPosition = newMarkerPos;

		Debug.DrawLine (targetStack[targetStack.Count - 1].transform.position-direction,targetStack[targetStack.Count - 1].transform.position,Color.blue);

		//Debug.DrawLine (targetStack [targetStack.Count - 1].transform.position,transform.parent.position, Color.red);

		if(targetStack.Count == 0)
		{
			gameObject.SetActive(false);
		}
	}
}
