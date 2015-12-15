using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISpawnerScript : MonoBehaviour {

	public bool respawn;
	public int maxAI;
	public int startAI;
	public float range;

	public GameObject AIPrefab;
	public GameObject squadLeader;

	private int currentAI;
	private Transform objective;

	public List<GameObject> squad;
	public Transform Objective { get { return objective; } set { objective = value; } }
	// Use this for initialization
	public int tier;

	public GameObject mom;

	public void Init () {

		currentAI = maxAI;
		Vector2 spawnPos;
		squad = new List<GameObject>(startAI);
		for(int i = 0; i < startAI; i++)
		{
			float distance = Random.Range(0.0f, range);
			float angle = Random.Range(0.0f, Mathf.PI * 2);

			spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			spawnPos = (spawnPos * distance) + (Vector2)transform.position;
			GameObject g = null;
			switch(AIPrefab.GetComponent<ShipBehaviourScript>().behaviour)
			{
			case ShipBehaviourScript.Behaviour.Grunt:
				if(i == 0 && squadLeader != null)
				{
					g = (GameObject)GameObject.Instantiate(squadLeader, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));
					squadLeader = g;
				}
				else
					g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));

				g.GetComponentInChildren<ShieldScript>().maxShieldAmount = (tier - 1) * 20;
				break;
			case ShipBehaviourScript.Behaviour.Cargo:

				if(objective.GetComponent<ObjectiveEvent>().ObjectiveContract.IsStoryContract)
					g = (GameObject)GameObject.Instantiate(mom, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));
				else
					g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));
				break;
			default:
				g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));
				break;

			}

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

		for(int i = 0; i < squad.Count; i++)
		{
			squad[i].GetComponent<AIShipScript>().squad = squad;
			squad[i].transform.parent = WarpScript.instance.currentPlanet.transform;
		}
	
	}

	void SpawnShip()
	{
		Vector2 spawnPos;

		float distance = Random.Range(0.0f, range);
		float angle = Random.Range(0.0f, Mathf.PI * 2);
		
		spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		spawnPos = (spawnPos * distance) + (Vector2)transform.position;
		GameObject g = null;
		switch(AIPrefab.GetComponent<ShipBehaviourScript>().behaviour)
		{
		case ShipBehaviourScript.Behaviour.Grunt:
				g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));
			
			//g.GetComponent<ShieldScript>().maxShieldAmount = tier * 20;
			//g.GetComponent<ShieldScript>().rechargeRate = 10 - tier;
			break;
		default:
			g = (GameObject)GameObject.Instantiate(AIPrefab, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360))));
			break;
			
		}
		
		AIShipScript ss = g.GetComponent<AIShipScript>();
		if(objective != null)
			ss.objective = objective.transform;
		ss.spawner = this;
		ss.Target = PlayerShipScript.player.transform;
		squad.Add(g);
		
		ss.InitWeapons();
		for( int j = 0; j < ss.WeaponSlots.Length && j < tier; j++ )
		{
			WeaponScript.WeaponType weapon = (WeaponScript.WeaponType) Random.Range(0, (int)WeaponScript.WeaponType.SCATTER_SHOT + 1);
			ss.WeaponSlots[ j ].SetWeapon( Instantiate( GameMaster.WeaponMngr.GetWeaponPrefab( weapon ) ) );
		}


		for(int i = 0; i < squad.Count; i++)
		{
			squad[i].GetComponent<AIShipScript>().squad = squad;
			squad[i].transform.parent = WarpScript.instance.currentPlanet.transform;
		}

		currentAI++;
	}
	
	// Update is called once per frame
	void Update () {

		currentAI = squad.Count;
		if(respawn && currentAI < maxAI)
			SpawnShip();

		for(int i = 0; i < squad.Count; i++)
		{
			if(squad[i] == null)
			{
				squad.RemoveAt(i);
				currentAI--;
			}
		}
		if(squad != null && squad.Count == 0)
			Destroy(this.gameObject);
	}
}
