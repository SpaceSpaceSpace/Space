using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttachmentPoint : MonoBehaviour {

    public ToggleGroup WeaponToggles;

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
        if(WeaponToggles.AnyTogglesOn())
		    mat.color = selectedColor;
	}
	void OnMouseExit(){
		mat.color = startColor;
	}
}
