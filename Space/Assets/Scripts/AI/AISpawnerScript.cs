using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISpawnerScript : MonoBehaviour {

	public int maxAI;
	public int startAI;
	public float range;

	public GameObject AIPrefab;
	public GameObject squadLeader;

	//private int currentAI;
	private Transform objective;

	public List<GameObject> squad;
	public Transform Objective { set { objective = value; } }
	// Use this for initialization
	public int tier;

	public void Init () {

		//currentAI = 0;
		Vector2 spawnPos;
		squad = new List<GameObject>(startAI);
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
				if(i == 0 && squadLeader != null)
				{
					g = (GameObject)GameObject.Instantiate(squadLeader, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));
					squadLeader = g;
				}
				else
					g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));

				AIShipScript ss = g.GetComponent<AIShipScript>();
				if(objective != null)
					ss.objective = objective.transform;
				ss.spawner = this;
				squad.Add(g);

				ss.InitWeapons();
				for( int j = 0; j < ss.WeaponSlots.Length && j < tier; j++ )
				{
					WeaponScript.WeaponType weapon = (WeaponScript.WeaponType) Random.Range(0, (int)WeaponScript.WeaponType.SCATTER_SHOT + 1);
					ss.WeaponSlots[ j ].SetWeapon( Instantiate( GameMaster.WeaponMngr.GetWeaponPrefab( weapon ) ) );
				}
			}
			else //(AIPrefab.GetComponent<ShipBehaviourScript>().behaviour == ShipBehaviourScript.Behaviour.Cargo)
			{
				g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));
				g.GetComponent<AIShipScript>().objective = objective;
				g.GetComponent<AIShipScript>().spawner = this;
				squad.Add(g);

				AIShipScript ss = g.GetComponent<AIShipScript>();
				if(objective != null)
					ss.objective = objective.transform;
				ss.spawner = this;
				squad.Add(g);
				
				ss.InitWeapons();
				for( int j = 0; j < ss.WeaponSlots.Length && j < tier; j++ )
				{
					WeaponScript.WeaponType weapon = (WeaponScript.WeaponType) Random.Range(0, (int)WeaponScript.WeaponType.SCATTER_SHOT + 1);
					ss.WeaponSlots[ j ].SetWeapon( Instantiate( GameMaster.WeaponMngr.GetWeaponPrefab( weapon ) ) );
				}
			}
		}

		for(int i = 0; i < squad.Count; i++)
		{
			squad[i].GetComponent<AIShipScript>().squad = squad;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < squad.Count; i++)
		{
			if(squad[i] == null)
				squad.RemoveAt(i);
		}
		if(squad != null && squad.Count == 0)
			Destroy(this.gameObject);
	}
}
