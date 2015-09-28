using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Contract
{
	public bool completed;
	public List<GameObject> objectives = new List<GameObject>();
	public List<GameObject> rewards = new List<GameObject>();
	private string description;
	private string targetImage;

	//Not sure whether to pass these items in the construction of the object or generate them randomly in constructor..?
	public Contract(List<GameObject> obj, List<GameObject> rew, string desc, string tImage)
	{
		completed = false;
		for (int i = 0; i < obj.Count; i++) 
		{
			objectives.Add(obj[i]);
		}
		for (int i = 0; i < rew.Count; i++) 
		{
			rewards.Add(rew[i]);
		}
		description = desc;
		//Code to load in image for the contract target(sprite?)
		targetImage = tImage;
	}

	public Contract()
	{
		completed = false;
		//objectives.Add ();// Add an enemy
		//rewards.Add ();// Add rewards
		description = "Go kill the enemy!";
		targetImage = "Image Directory";
	}

	public void CompleteContract()
	{
		completed = true;
		GrantRewards ();
	}

	void SpawnContractObjectives()
	{
		for (int i = 0; i < objectives.Count; i++) 
		{
			if(objectives[i].transform.position != null)//if there's a position set, spawn there
				GameObject.Instantiate(objectives[i], objectives[i].transform.position, Quaternion.identity);
			else//No position set, spawn at world center (for now)
				GameObject.Instantiate(objectives[i], new Vector3(0,0,0), Quaternion.identity);
		}
	}

	void GrantRewards()//Not Sure how we are doing rewards yet..? :)
	{
		for (int i = 0; i < rewards.Count; i++) 
		{
			GameObject.Instantiate(rewards[i]);
		}
	}
}
