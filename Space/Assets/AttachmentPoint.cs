using UnityEngine;
using System.Collections;

public class AttachmentPoint : MonoBehaviour {

	Material mat;

	Color startColor;
	Color selectedColor = Color.blue;

	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().material;
		startColor = mat.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver(){
		mat.color = selectedColor;
	}
	void OnMouseExit(){
		mat.color = startColor;
	}
}
