using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttachmentPoint : MonoBehaviour {

    public ToggleGroup WeaponToggles;

	SpriteRenderer spriteRenderer;

	Color startColor;
	Color selectedColor = Color.blue;

	PlayerShipScript ship;

	bool attaching;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		startColor = spriteRenderer.color;

		//Ship is attached to the parent game object
		//ship = transform.parent.gameObject.GetComponent<PlayerShipScript>();

		attaching = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver(){

		Debug.Log("test");

        if(WeaponToggles.AnyTogglesOn())
		{
			spriteRenderer.color = selectedColor;
			Vector2 pointPos = transform.position;

			// If it's left mouse button, attach
			if(Input.GetMouseButtonDown(0) && !attaching)
			{
				Toggle selectedToggle = WeaponToggles.ActiveToggles().FirstOrDefault();
				AttachmentToggle attachmentToggle = selectedToggle.GetComponent<AttachmentToggle>();

				GameObject attachment = attachmentToggle.Attachment;

				//If the ship has an attachment here we should remove it

				if(ship.Attachments.ContainsKey(pointPos))
				{
					GameObject currentAttachment = ship.Attachments[pointPos];
					Destroy(currentAttachment);
				}

				//Now add the selected attachment to the ships' attached Weapons dictionary and instantiate it
				GameObject attachmentClone = GameObject.Instantiate(attachment);
				Transform attachmentTransform = attachmentClone.transform;

				attachmentTransform.position = new Vector3(pointPos.x, pointPos.y, ship.transform.position.z - .1f);
				attachmentTransform.SetParent(ship.transform);
				ship.Attachments[pointPos] = attachmentClone;

				//Don't let this happen again until the mouse is lifted
				attaching = true;
			}

			//If it's right mouse button, clear
			else if(Input.GetMouseButtonDown(1) && !attaching)
			{
				if(ship.Attachments.ContainsKey(pointPos))
				{
					GameObject currentAttachment = ship.Attachments[pointPos];
					Destroy(currentAttachment);
				}
			}
			else if(Input.GetMouseButtonUp(0) && Input.GetMouseButtonUp(1))
			{
				attaching = false;
			}
		}
	}
	void OnMouseExit(){
		spriteRenderer.color = startColor;
		attaching = false;
	}
}
