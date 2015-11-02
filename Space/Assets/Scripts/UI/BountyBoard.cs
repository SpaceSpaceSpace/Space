using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class BountyBoard : MonoBehaviour {

	public Text targetName;
	public Text title;
	public Text description;
	public Text reward;
	public Image portrait;
	public Image shipImage;
	public GameObject scrollView;
	public GameObject buttonPrefab;
	private List<Contract> currentContracts;
	private int currentSelectedContract;

	void OnEnable()
	{
		currentContracts = new List<Contract> ();

		/*
		for(int i = 0; i < 10; i++)
		{
			Contract contract = new Contract();
			currentContracts.Add(contract);
		}
		*/

		currentContracts.Add (new Contract ("Joeba da Butt", "A warrant has been put out for the destruction of $name, The $title. This criminal has been found guilty of crimes against space. Their destruction of dozens of space mines have killed thousands. Kill this target and return to $thissector for reward.",
		                                  "Space Slug Gangsta", "190,000 Grappels"));
		currentContracts.Add (new Contract ("Fart Face McGee", "He terrorized a sub-intelligent species, teach him a lesson",
		                                    "Galatic Gangster", "80,000 Space Dollars"));
		currentContracts.Add (new Contract ("Hokun the Broad", "You must hunt down this monster gangster and his crew",
		                                    "Crimepunk", "19,000 Space Dollars"));
		currentContracts.Add (new Contract ("Gadarble", "A bounty has been put on the head of The $title $name. This criminal has been found guilty of antennae mutilation across the galaxy. Target is dangerous: do not take alive.",
		                                    "Space Slug Gangsta", "15,000 Space Dollars"));
		currentContracts.Add (new Contract ("Skinface", "New, evolving life has been found in $randomsector. This new species must be protected under the Galactic Code. However, The $title $name has been caught rampantly destroying and torturing this new race. Target must be liquidated.",
		                                    "Eyebrow Waggler", "12,500 Space Dollars"));

		for(int i = 0; i < 5; i++)
		{
			Contract contract = new Contract();
			currentContracts.Add(contract);
		}

		PopulateButtons ();
	}

	private void PopulateButtons()
	{
		for(int i = 0; i < currentContracts.Count; i++)
		{
			int _i = i;
			GameObject button = Instantiate(buttonPrefab) as GameObject;
			button.name = "contract" + i;
			button.transform.SetParent(scrollView.transform,false);
			button.gameObject.GetComponent<Button>().onClick.AddListener(()=>SetBountyValues(_i));
			button.GetComponentInChildren<Text>().text = currentContracts[i].Name;
		}

		SetBountyValues (0);
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
		if(currentSelectedContract != -1)
		{
			GameMaster.playerData.AcceptContract (currentContracts[currentSelectedContract]);

			GameObject button = scrollView.transform.FindChild ("contract" + currentSelectedContract).gameObject;

			Destroy (scrollView.transform.FindChild("contract" + currentSelectedContract).gameObject);

			try
			{
				string indexString = scrollView.transform.GetChild(1).name;
				int index = int.Parse(indexString[indexString.Length-1].ToString());

				SetBountyValues (index);
			}
			catch(Exception e)
			{
				SetBlankValues();
			}
		}
	}

	public void SetBountyValues(int index)
	{
		currentSelectedContract = index;
		Dictionary<string,string> values = currentContracts [index].GetContractDetails ();

		SetName (values ["Name"]);
		SetTitle (values ["Title"]);
		SetDescription (values ["Description"]);
		SetReward (values ["Reward"]);
	}

	private void SetBlankValues()
	{
		currentSelectedContract = -1;
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
