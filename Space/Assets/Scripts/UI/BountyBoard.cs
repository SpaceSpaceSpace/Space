using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

		for(int i = 0; i < 10; i++)
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
		GameMaster.playerData.AcceptContract (currentContracts[currentSelectedContract]);

		GameObject button = scrollView.transform.FindChild ("contract" + currentSelectedContract).gameObject;

		Destroy (scrollView.transform.FindChild("contract" + currentSelectedContract).gameObject);
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
