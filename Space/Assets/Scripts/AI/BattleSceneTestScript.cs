using UnityEngine;
using System.Collections;

public class BattleSceneTestScript : MonoBehaviour {

	public GameObject copSpawner;
	public GameObject crimSpawner;

	public int numSpawners;
	// Use this for initialization
	void Start () {
		GameObject g;
		for(int i = 0; i < numSpawners; i++)
		{
			if(i%2 == 0)
				g = (GameObject)GameObject.Instantiate(copSpawner, new Vector2(Random.Range(-300, 300), Random.Range(-300, 300)), Quaternion.identity);
			else
				g = (GameObject)GameObject.Instantiate(crimSpawner, new Vector2(Random.Range(-300, 300), Random.Range(-300, 300)), Quaternion.identity);
			g.GetComponent<AISpawnerScript>().Init();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
