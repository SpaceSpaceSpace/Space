using UnityEngine;
using System.Collections;
using WyrmTale;

public class ObjectiveRescue : Objective {
	
	private static GameObject rescueSpawner = null;
	private static GameObject crimSpawner = null;
	private bool enemySpawn;
	private int tier;
	
	public ObjectiveRescue()
	{
		
	}
	
	public override void SetupObjective(GameObject objectiveManager, int a_tier)
	{
		Position = new Vector2(Random.Range(-150.0f, 150.0f), Random.Range(-150.0f, 150.0f));
		
		if (rescueSpawner == null)
			rescueSpawner = Resources.Load("RescueSpawner") as GameObject;
		if(crimSpawner == null)
			crimSpawner = Resources.Load("AISpawner") as GameObject;
		
		AISpawnerScript spawnerScript = rescueSpawner.GetComponent<AISpawnerScript>();
		spawnerScript.tier = tier;

		enemySpawn = false;
		tier = a_tier;
		rescueSpawner = (GameObject)GameObject.Instantiate(rescueSpawner, Position, Quaternion.identity);

		Position = GameObject.Find("SpaceStore").transform.position;
		rescueSpawner.GetComponent<AISpawnerScript>().Objective = objectiveManager.transform;
		rescueSpawner.GetComponent<AISpawnerScript>().Init();
		//objectiveManager.GetComponent<ObjectiveEvent>().ObjectiveContract.SetUIMarker(rescueSpawner.GetComponent<AISpawnerScript>().squad[0]);
	}
	
	public override void ObjectiveUpdate() 
	{
		if(crimSpawner != null && rescueSpawner != null && rescueSpawner.GetComponent<AISpawnerScript>().squad[0] != null)
		{
			Vector2 rescuePos = rescueSpawner.GetComponent<AISpawnerScript>().squad[0].transform.position;
			//float rescueDistance = Vector2.Distance(PlayerShipScript.player.stationMarker.transform.position, rescuePos);
			// Spawns ships when the player gets close
			if(Vector2.Distance(rescuePos, PlayerShipScript.player.transform.position) < 20.0f && !enemySpawn)
			{
				enemySpawn = true;
				Vector2 spawnPos = rescuePos;
				spawnPos += rescuePos - (Vector2)PlayerShipScript.player.transform.position;
				crimSpawner = (GameObject)GameObject.Instantiate(crimSpawner, spawnPos, Quaternion.identity);
				crimSpawner.GetComponent<AISpawnerScript>().tier = tier;
				crimSpawner.GetComponent<AISpawnerScript>().squadLeader = null;
				crimSpawner.GetComponent<AISpawnerScript>().startAI = 1 + tier;
				crimSpawner.GetComponent<AISpawnerScript>().Init();
				foreach(GameObject g in crimSpawner.GetComponent<AISpawnerScript>().squad)
				{
					g.GetComponent<AIShipScript>().Target = rescueSpawner.GetComponent<AISpawnerScript>().squad[0].transform;
				}

			}
		}
	}
	
	public override void HitObjective(Collider2D collider)
	{
		if (collider.name.Contains("RescueShip"))
		{
			completed = true;
			GameObject.Destroy(rescueSpawner.GetComponent<AISpawnerScript>().squad[0]);
		}


	}
	
	protected override JSON ToJSON()
	{
		JSON js = new JSON();
		js["Type"] = "Rescue";
		
		return js;
	}
	protected override void FromJSON(JSON js)
	{

	}
}
