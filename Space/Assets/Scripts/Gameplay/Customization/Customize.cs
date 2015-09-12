using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Customize : MonoBehaviour {

    public PlayerShipScript ship;
    public ToggleGroup WeaponToggles;
	
	void Start () {
		ship = PlayerShipScript.player;

		ship.Dock();

		PopulateAttachmentPoints();
	}

	void OnLevelWasLoaded()
	{
		ship.Dock();

		ship = PlayerShipScript.player;

		PopulateAttachmentPoints();

		CenterShip ();

		ClearAttachmentPoints();
	}

	void Update()
	{
		//Swap to Nick's playground
		if(Input.GetKey(KeyCode.F1))
		{
			ship.Undock();
			ClearAttachmentPoints();
			Application.LoadLevel("Nick's Playground");
		}
	}

	private void CenterShip()
	{
		ship.transform.position = new Vector2(0,0);
		ship.transform.eulerAngles = new Vector2(0,0);

		Rigidbody2D rigidbody = ship.GetComponent<Rigidbody2D>();
		if(rigidbody)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = 0;
		}
	}

	private void PopulateAttachmentPoints()
	{		
		Transform shipTransform = ship.transform;
		
		//For each empty attachment point on the ship, draw an indicator that you can click to change the attachements
		foreach(Vector2 v in ship.AttachmentPoints){
			GameObject indicator = Instantiate((GameObject)Resources.Load("ShipPrefabs/AttachmentIndicator"));
			AttachmentPoint attachment = indicator.GetComponent<AttachmentPoint>();
			indicator.transform.parent = shipTransform;
			
			//Send the toggles to the attachment point so that it knows how to react when a weapon is selected
			attachment.WeaponToggles = WeaponToggles;
			
			Vector3 pos = v;
			pos.z = ship.transform.position.z - .2f;
			
			indicator.transform.position = pos;
		}
	}

	private void ClearAttachmentPoints()
	{
		Transform transform = ship.transform;

		//Loop through all children and if they have an AttachmentPoint script, delete them
		for(int i = 0; i < transform.childCount; i++)
		{
			GameObject child = transform.GetChild(i).gameObject;
			if(child.GetComponent<AttachmentPoint>())
			{
				Destroy (child);
			}
		}
	}	
}
