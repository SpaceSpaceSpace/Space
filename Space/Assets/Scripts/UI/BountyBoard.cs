using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BountyBoard : MonoBehaviour {

	public Text targetName;
	public Text title;
	public Text description;
	public Text reward;
	public Image portrait;
	public Image shipImage;

	void OnEnable()
	{

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
