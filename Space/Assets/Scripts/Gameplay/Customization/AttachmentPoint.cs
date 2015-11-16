using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttachmentPoint : MonoBehaviour {

    public ToggleGroup WeaponToggles;

	[HideInInspector]
	public int Index;

	SpriteRenderer spriteRenderer;

	Color startColor;
	Color selectedColor = Color.blue;

	PlayerShipScript ship;

	bool attaching;

	// Use this for initialization
	void Start () {
		ship = PlayerShipScript.player;

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

        //if(WeaponToggles.AnyTogglesOn())
		{
			spriteRenderer.color = selectedColor;
			//Vector2 pointPos = transform.position;

			// If it's left mouse button, attach
			if(Input.GetMouseButtonDown(0) && !attaching)
			{
				Toggle selectedToggle = WeaponToggles.ActiveToggles().FirstOrDefault();
				AttachmentToggle attachmentToggle = selectedToggle.GetComponent<AttachmentToggle>();

				WeaponInfo attachment = attachmentToggle.Attachment;

				//If the ship has an attachment here we should remove it
				/*if(ship.WeaponSlots[Index] != null)
				{
					WeaponScript currentAttachment = ship.WeaponSlots[Index].Weapon;
					Destroy(currentAttachment.gameObject);
				}

				//Now add the selected attachment to the ships' attached Weapons dictionary and instantiate it
				GameObject attachmentClone = GameObject.Instantiate(attachment);
				Transform attachmentTransform = attachmentClone.transform;

				attachmentTransform.position = new Vector3(pointPos.x, pointPos.y, ship.transform.position.z - .1f);
				attachmentTransform.SetParent(ship.WeaponSlots[Index].transform);
				ship.WeaponSlots[Index] = attachmentClone.GetComponent<WeaponSlot>();*/

				//Don't let this happen again until the mouse is lifted
				attaching = true;

				// Order matters here, kids
				Destroy(selectedToggle.gameObject);
				GameMaster.playerData.playerInventory.RemoveWeapon( attachment );
				ship.WeaponSlots[Index].SetWeapon( attachment.SpawnWeapon() );
			}

			//If it's right mouse button, clear
			else if(Input.GetMouseButtonDown(1) && !attaching)
			{
				ship.WeaponSlots[Index].RemoveWeapon();
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
