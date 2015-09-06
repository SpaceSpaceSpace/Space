using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttachmentPoint : MonoBehaviour {

    public ToggleGroup WeaponToggles;

	Material mat;

	Color startColor;
	Color selectedColor = Color.blue;

	Ship ship;

	bool attaching;

	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().material;
		startColor = mat.color;

		//Ship is attached to the parent game object
		ship = transform.parent.gameObject.GetComponent<Ship>();

		attaching = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver(){
        if(WeaponToggles.AnyTogglesOn())
		{
		    mat.color = selectedColor;
			Vector2 pointPos = transform.position;

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
				attachmentClone.transform.position = pointPos;
				attachmentClone.transform.SetParent(ship.transform);
				ship.Attachments[pointPos] = attachmentClone;

				//Don't let this happen again until the mouse is lifted
				attaching = true;
			}
			else if(Input.GetMouseButtonUp(0))
			{
				attaching = false;
			}
		}
	}
	void OnMouseExit(){
		mat.color = startColor;
		attaching = false;
	}
}
