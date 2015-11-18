using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponDock : MonoBehaviour {

	public PlayerShipScript player;
	public GameObject textPrefab;
	public Text[] weaponIndicators;
	public int weaponAttachments = 0;

	public Color activeColor;
	public Color greyedOutColor;

	public void UpdateWeaponDockUI()
	{
		weaponAttachments = 0;

		for(int i = 0; i < player.WeaponSlots.Length; i++)
		{
			foreach(Transform child in transform.GetChild(i))
			{
				Destroy(child.gameObject);
			}

			if(player.WeaponSlots[i].Weapon == null)
			{
				continue;
			}

			weaponIndicators[i].color = activeColor;

			Sprite weaponSprite = player.WeaponSlots[i].Weapon.GetComponent<SpriteRenderer>().sprite;

			GameObject weaponImage = new GameObject();
			weaponImage.AddComponent<Image>().sprite = weaponSprite;
			weaponImage.GetComponent<Image>().preserveAspect = true;
			weaponImage.name = player.WeaponSlots[i].Weapon.GetComponent<WeaponScript>().name;
			weaponImage.transform.Rotate (new Vector3(0f,0f,90f));
			weaponImage.transform.SetParent(transform.GetChild(i),false);

			GameObject weaponName = Instantiate(textPrefab);
			weaponName.transform.SetParent(transform.GetChild(i),false);
			weaponName.GetComponent<Text>().text =  player.WeaponSlots[i].Weapon.name.Remove(player.WeaponSlots[i].Weapon.name.IndexOf("(Clone)"));
			weaponName.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,85f);

			weaponAttachments++;
		}
	}

	public void ToggleWeaponColor(int index)
	{
		if(weaponIndicators[index].color == greyedOutColor)
		{
			weaponIndicators[index].color = activeColor; 
		}
		else
		{
			weaponIndicators[index].color = greyedOutColor; 
		}
	}
}
