using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class StoreBoard : MonoBehaviour {

	public Text targetName;
	public Text title;
	public Text reward;
	public Image portrait;
	public Image shipImage;
	public GameObject scrollView;
	public GameObject buttonPrefab;
	public GameObject statLocation;
	public GameObject statPrefab;
	private List<WeaponInfo> currentWeapons;
	private int currentSelectedWeapon;

	void OnEnable()
	{
		currentWeapons = new List<WeaponInfo> ();

		for(int i = 0; i < 5; i++)
		{
			GameObject g = GameMaster.WeaponMngr.GetWeaponPrefab (WeaponScript.WeaponType.LASER_MACHINE_GUN);
			WeaponModifier.ModifierNames modifier = (WeaponModifier.ModifierNames) UnityEngine.Random.Range((int)WeaponModifier.ModifierNames.PROJ_WEP_START, (int)WeaponModifier.ModifierNames.PROJ_WEP_END);
			currentWeapons.Add (g.GetComponent<WeaponScript> ().ToInfo (modifier));
		}

		//GameObject g2 = GameMaster.WeaponMngr.GetWeaponPrefab (WeaponScript.WeaponType.MISSILE_LAUNCHER);
		//currentWeapons.Add (g2.GetComponent<WeaponScript> ().ToInfo(WeaponModifier.ModifierNames.));

		PopulateButtons ();
	}

	private void PopulateButtons()
	{
		for(int i = 0; i < currentWeapons.Count; i++)
		{
			int _i = i;
			GameObject button = Instantiate(buttonPrefab) as GameObject;
			button.name = i.ToString();
			button.transform.SetParent(scrollView.transform,false);
			button.gameObject.GetComponent<Button>().onClick.AddListener(()=>SetStoreValues(_i));
			button.GetComponentInChildren<Text>().text = currentWeapons[i].Name;
		}

		SetStoreValues (0);
	}

	public void DestroyButtons()
	{
		Button[] buttons = scrollView.GetComponentsInChildren<Button> ();
		foreach(Button _button in buttons)
		{
			Destroy(_button.gameObject);
		}
	}
	
	public void AcceptContract()
	{
		if(currentSelectedWeapon != -1)
		{
			//GameMaster.playerData.AcceptContract (currentWeapons[currentSelectedWeapon]);

			GameObject button = scrollView.transform.FindChild (currentSelectedWeapon.ToString()).gameObject;

			if(button != null)
			{
				Destroy (button);
			}

			if(scrollView.transform.childCount > 1)
			{
				string indexString = scrollView.transform.GetChild(1).name;
				int index = int.Parse(indexString);

				SetStoreValues (index);
			}
			else
			{
				SetBlankValues();
			}
		}
	}

	public void SetStoreValues(int index)
	{
		foreach(Transform t in statLocation.transform)
		{
			Destroy(t.gameObject);
		}

		currentSelectedWeapon = index;

		targetName.text = currentWeapons [currentSelectedWeapon].Name;

		foreach(KeyValuePair<string,float> key in currentWeapons[currentSelectedWeapon].attributes)
		{
			GameObject stat = Instantiate(statPrefab);
			stat.transform.SetParent(statLocation.transform,false);
			Text[] textObjects = stat.GetComponentsInChildren<Text>();
			textObjects[0].text = key.Key;
			textObjects[1].text = key.Value.ToString();
		}
	}

	private void SetBlankValues()
	{
		currentSelectedWeapon = -1;
		SetName ("-----");
		SetTitle ("-----");
		SetReward("-----");
	}

	public void SetName(string p_Name)
	{
		targetName.text = p_Name;
	}

	public void SetTitle(string p_Title)
	{
		title.text = p_Title;
	}

	public void SetReward(string p_Reward)
	{
		reward.text = p_Reward;
	}
}
