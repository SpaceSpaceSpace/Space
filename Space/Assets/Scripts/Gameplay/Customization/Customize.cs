using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Customize : MonoBehaviour {

    public PlayerShipScript ship;
    public ToggleGroup WeaponToggles;

	private bool customizing = false;
	private float oldCameraSize;

	void Update()
	{
		if(GameMaster.CurrentGameState == GameState.Customization && !customizing)
			EnterCustomization();
		if(GameMaster.CurrentGameState != GameState.Customization && customizing)
			EndCustomization();

		if(customizing)
			CenterShip();
	}

	void OnMouseOver()
	{
		Debug.Log("test");
	}

	private void EnterCustomization()
	{
		//Focus camera on player
		Camera cam = Camera.main;
		oldCameraSize = cam.orthographicSize;

		cam.orthographicSize = 1.5f;

		ship = PlayerShipScript.player;
		
		ship.Dock();
		CenterShip();

		PopulateAttachmentPoints();

		WeaponToggles.gameObject.SetActive(true);

		customizing = true;
	}

	private void EndCustomization()
	{
		Camera cam = Camera.main;
		cam.orthographicSize = oldCameraSize;

		ClearAttachmentPoints();

		ship.Undock();

		WeaponToggles.gameObject.SetActive(false);

		customizing = false;
	}

	private void CenterShip()
	{
		ship.transform.position = transform.position;
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
		for(int i = 0; i < ship.AttachmentPoints.Count; i++)
		{
			Vector2 v = ship.AttachmentPoints[i];

			GameObject indicator = Instantiate(Resources.Load("ShipPrefabs/AttachmentIndicator")) as GameObject;
			AttachmentPoint attachment = indicator.GetComponent<AttachmentPoint>();
			attachment.Index = i;
			indicator.transform.position = ship.transform.position + (Vector3)v;
			indicator.transform.parent = shipTransform;
			
			//Send the toggles to the attachment point so that it knows how to react when a weapon is selected
			attachment.WeaponToggles = WeaponToggles;
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
