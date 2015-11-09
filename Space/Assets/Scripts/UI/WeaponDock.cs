using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponDock : MonoBehaviour {

	public PlayerShipScript player;
	public GameObject textPrefab;

	public int weaponAttachments = 0;

	public void UpdateWeaponDockUI()
	{
		weaponAttachments = 0;

		for(int i = 0; i < player.Attachments.Count; i++)
		{
			foreach(Transform child in transform.GetChild(i))
			{
				Destroy(child.gameObject);
			}

			if(player.Attachments[i] == null)
			{
				continue;
			}

			Sprite weaponSprite = player.Attachments[i].GetComponent<SpriteRenderer>().sprite;

			GameObject weaponImage = new GameObject();
			weaponImage.AddComponent<Image>().sprite = weaponSprite;
			weaponImage.GetComponent<Image>().preserveAspect = true;
			weaponImage.name = player.Attachments[i].GetComponent<WeaponScript>().name;
			weaponImage.transform.Rotate (new Vector3(0f,0f,90f));
			weaponImage.transform.SetParent(transform.GetChild(weaponAttachments),false);

			GameObject weaponName = Instantiate(textPrefab);
			weaponName.transform.SetParent(transform.GetChild(weaponAttachments),false);
			weaponName.GetComponent<Text>().text =  player.Attachments[i].name.Remove(player.Attachments[i].name.IndexOf("(Clone)"));
			weaponName.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,85f);

			weaponAttachments++;
		}
	}
}
