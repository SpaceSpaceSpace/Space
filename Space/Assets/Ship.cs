using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	public List<Vector2> AttachmentPoints;
	public Dictionary<Vector2, GameObject> Attachments;

	// Use this for initialization
	void Start () {
		AttachmentPoints = new List<Vector2>();
		Attachments = new Dictionary<Vector2, GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
