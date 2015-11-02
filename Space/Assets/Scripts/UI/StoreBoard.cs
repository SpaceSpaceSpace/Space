using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class StoreBoard : MonoBehaviour {

	public Text targetName;
	public Text title;
	public Text description;
	public Text reward;
	public Image portrait;
	public Image shipImage;
	public GameObject scrollView;
	public GameObject buttonPrefab;
	private List<WeaponInfo> currentWeapons;
	private int currentSelectedWeapon;

	void OnEnable()
	{

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
		currentSelectedWeapon = index;
		//Dictionary<string,string> values = currentContracts [index].GetContractDetails ();

		//SetName (values ["Name"]);
		//SetTitle (values ["Title"]);
		//SetDescription (values ["Description"]);
		//SetReward (values ["Reward"]);
	}

	private void SetBlankValues()
	{
		currentSelectedWeapon = -1;
		SetName ("-----");
		SetTitle ("-----");
		SetDescription ("-----");
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

	public void SetDescription(string p_Des)
	{
		description.text = p_Des;
	}

	public void SetReward(string p_Reward)
	{
		reward.text = p_Reward;
	}
}
