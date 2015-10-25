﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISpawnerScript : MonoBehaviour {

	public int maxAI;
	public int startAI;
	public float range;

	public GameObject AIPrefab;
	public GameObject squadLeader;

	private int currentAI;
	//private GameObject squadLeader;
	//
	//public GameObject SquadLeader
	//{
	//	get{return squadLeader;}
	//}

	// Use this for initialization
	void Awake () {

		currentAI = 0;
		Vector2 spawnPos;
		List<GameObject> squad = new List<GameObject>(startAI);
		for(int i = 0; i < startAI; i++)
		{
			float distance = Random.Range(0.0f, range);
			float angle = Random.Range(0.0f, Mathf.PI * 2);

			spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			spawnPos = (spawnPos * distance) + (Vector2)transform.position;
			GameObject g = null;
			// Check which kind of ships are loaded into this spawner, may be changed to switch statement later
			if(AIPrefab.GetComponent<ShipBehaviourScript>().behaviour == ShipBehaviourScript.Behaviour.Grunt)
			{
				if(i == 0)
				{
					g = (GameObject)GameObject.Instantiate(squadLeader, spawnPos, Quaternion.identity);
					squadLeader = g;
				}
				else
					g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.identity);

				g.GetComponent<AIShipScript>().objective = transform; 
				squad.Add(g);
			}
			else if (AIPrefab.GetComponent<ShipBehaviourScript>().behaviour == ShipBehaviourScript.Behaviour.Cargo)
			{
				g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.identity);
				squad.Add(g);
			}
		}

		for(int i = 0; i < squad.Count; i++)
		{
			squad[i].GetComponent<AIShipScript>().squad = squad;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
