using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BountyBoard : MonoBehaviour {

	public Text name;
	public Text title;
	public Text description;
	public Text reward;
	public Image portrait;
	public Image shipImage;

	public void setName(string p_Name)
	{
		name = p_Name;
	}

	public void setTitle(string p_Title)
	{
		title = p_Title;
	}

	public void setDescription(string p_Des)
	{
		description = p_Des;
	}

}
