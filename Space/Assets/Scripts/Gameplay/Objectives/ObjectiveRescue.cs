using UnityEngine;
using System.Collections;
using WyrmTale;

public class ObjectiveRescue : Objective {
	
	private static GameObject rescueSpawner = null;
	private static GameObject crimSpawner = null;
	private float crimSpawnTimer;
	
	public ObjectiveRescue()
	{
		
	}
	
	public override void SetupObjective(GameObject objectiveManager, int tier)
	{
		Position = new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
		
		if (rescueSpawner == null)
			rescueSpawner = Resources.Load("RescueSpawner") as GameObject;
		if(crimSpawner == null)
			crimSpawner = Resources.Load("AISpawner") as GameObject;
		
		AISpawnerScript spawnerScript = rescueSpawner.GetComponent<AISpawnerScript>();
		spawnerScript.tier = tier;
		
		crimSpawnTimer = 0.0f;
		rescueSpawner = (GameObject)GameObject.Instantiate(rescueSpawner, Position, Quaternion.identity);

		Position = PlayerShipScript.player.stationMarker.transform.position;
		rescueSpawner.GetComponent<AISpawnerScript>().Objective = objectiveManager.transform;
		rescueSpawner.GetComponent<AISpawnerScript>().Init();
	}
	
	public override void ObjectiveUpdate() 
	{

	}
	
	public override void HitObjective(Collider2D collider)
	{
		if (collider.name.Contains("RescueShip"))
			completed = true;
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
