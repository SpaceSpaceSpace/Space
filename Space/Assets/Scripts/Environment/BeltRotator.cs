using UnityEngine;
using System.Collections;

public class BeltRotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float changedTime = Time.deltaTime * (Mathf.PI * 2)/2.0f;
		transform.Rotate (new Vector3(0.0f,0.0f,changedTime));
	}
}
