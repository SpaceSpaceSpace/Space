using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponDock : MonoBehaviour {

	public PlayerShipScript player;

	public void UpdateWeaponDockUI()
	{
		for(int i = 0; i < player.Attachments.Count; i++)
		{
			if(player.Attachments[i] == null)
			{
				continue;
			}

			Sprite weaponSprite = player.Attachments[i].GetComponent<SpriteRenderer>().sprite;

			GameObject weaponImage = new GameObject();
			weaponImage.AddComponent<Image>().sprite = weaponSprite;
			weaponImage.GetComponent<Image>().preserveAspect = true;
			weaponImage.transform.SetParent(transform.GetChild(i),false);
		}
	}
}
