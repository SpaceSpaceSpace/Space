using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISpawnerScript : MonoBehaviour {

	public int maxAI;
	public int startAI;
	public float range;

	public GameObject AIPrefab;
	public Sprite leaderSprite;

	private int currentAI;
	private GameObject squadLeader;

	public GameObject SquadLeader
	{
		get{return squadLeader;}
	}

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
			GameObject g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.identity); 
			if(i == 0)
			{
				g.GetComponent<ShipBehaviourScript>().behaviour = ShipBehaviourScript.Behaviour.Leader;
				g.GetComponent<SpriteRenderer>().sprite = leaderSprite;
				g.transform.FindChild("Blip").GetComponent<SpriteRenderer>().color = Color.yellow;
				squadLeader = g;
			}
			else
				g.GetComponent<ShipBehaviourScript>().behaviour = ShipBehaviourScript.Behaviour.Grunt;

			g.GetComponent<AIShipScript>().objective = transform; 
			squad.Add(g);
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
