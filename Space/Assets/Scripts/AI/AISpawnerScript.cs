using UnityEngine;
using System.Collections;

public class AISpawnerScript : MonoBehaviour {

	public int maxAI;
	public int startAI;
	public float range;

	public GameObject AIPrefab;
	public Sprite leaderSprite;

	private int currentAI;
	// Use this for initialization
	void Start () {

		currentAI = 0;
		Vector2 spawnPos;
		GameObject[] squad = new GameObject[startAI];
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
			}
			else
				g.GetComponent<ShipBehaviourScript>().behaviour = ShipBehaviourScript.Behaviour.Grunt;

			g.GetComponent<AIShipScript>().objective = transform; 
			squad[i] = g;
		}

		for(int i = 0; i < squad.Length; i++)
		{
			squad[i].GetComponent<AIShipScript>().squad = squad;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
