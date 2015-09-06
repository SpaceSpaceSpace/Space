using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Customize : MonoBehaviour {

    public Ship ship;
    public ToggleGroup WeaponToggles;

	// Use this for initialization
	void Start () {
		Transform shipTransform = ship.transform;

		//For each empty attachment point on the ship, draw an indicator that you can click to change the attachements
		foreach(Vector2 v in ship.AttachmentPoints){
			GameObject indicator = Instantiate((GameObject)Resources.Load("ShipPrefabs/AttachmentIndicator"));
			AttachmentPoint attachment = indicator.GetComponent<AttachmentPoint>();
			indicator.transform.parent = shipTransform;


            //Send the toggles to the attachment point so that it knows how to react when a weapon is selected
            attachment.WeaponToggles = WeaponToggles;

			Vector3 pos = v;
			pos.z = ship.transform.position.z - .1f;

			indicator.transform.position = pos;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        
    }
}
