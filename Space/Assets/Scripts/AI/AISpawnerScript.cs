using UnityEngine;
using System.Collections;

public class AISpawnerScript : MonoBehaviour {

	public int maxAI;
	public int startAI;
	public float range;

	public GameObject AIPrefab;

	private int currentAI;
	// Use this for initialization
	void Start () {

		currentAI = 0;
		Vector2 spawnPos;
		for(int i = 0; i < startAI; i++)
		{
			float distance = Random.Range(0.0f, range);
			float angle = Random.Range(0.0f, Mathf.PI * 2);

			spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			spawnPos = (spawnPos * distance) + (Vector2)transform.position;

			GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.identity); 
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
