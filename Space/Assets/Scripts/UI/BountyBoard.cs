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
			button.transform.parent = scrollView.transform;
			button.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			button.gameObject.GetComponent<Button>().onClick.AddListener(()=>SetBountyValues(_i));
			button.GetComponentInChildren<Text>().text = currentContracts[i].Name;
		}
	}

	public void SetBountyValues(int index)
	{
		Debug.Log (index);
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
