using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Customize : MonoBehaviour {

    public PlayerShipScript ship;
    public ToggleGroup WeaponToggles;

	private bool customizing = false;

	void Update()
	{
		if(GameMaster.CurrentGameState == GameState.Customization && !customizing)
			EnterCustomization();
		if(GameMaster.CurrentGameState != GameState.Customization && customizing)
			EndCustomization();
	}

	private void EnterCustomization()
	{
		ship = PlayerShipScript.player;
		
		ship.Dock();
		
		PopulateAttachmentPoints();
		
		CenterShip ();

		WeaponToggles.gameObject.SetActive(true);

		customizing = true;
	}

	private void EndCustomization()
	{
		ClearAttachmentPoints();

		ship.Undock();

		WeaponToggles.gameObject.SetActive(false);

		customizing = false;
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
		foreach(Vector2 v in ship.AttachmentPoints)
		{
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
