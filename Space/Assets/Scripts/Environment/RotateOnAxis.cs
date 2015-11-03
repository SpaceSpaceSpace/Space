using UnityEngine;
using System.Collections;

public class RotateOnAxis : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * (Time.deltaTime/3));
	}
}
