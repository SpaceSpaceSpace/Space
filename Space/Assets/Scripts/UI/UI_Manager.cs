﻿using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour {

	public static UI_Manager instance;
	public GameObject BountyBoard;

	// Use this for initialization
	void Start () {
		instance = this;
	}

	void PassContractsToSpaceStation()
	{

	}

	public void DisplayBountyBoard()
	{
		BountyBoard.SetActive (!BountyBoard.activeSelf);
	}
}
