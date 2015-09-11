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
		Vector3 direction = transform.parent.position - targetStack[targetStack.Count - 1].transform.position;

		Debug.Log (targetStack[targetStack.Count - 1].transform.position + " + " + transform.parent.position + " = " + direction);

		Debug.DrawLine (transform.parent.position,direction,Color.blue);

		Debug.DrawLine (targetStack [targetStack.Count - 1].transform.position,transform.parent.position, Color.red);

		if(targetStack.Count == 0)
		{
			gameObject.SetActive(false);
		}
	}
}
