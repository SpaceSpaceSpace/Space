using UnityEngine;
using WyrmTale;

public class ObjectiveEscortCargo : Objective
{
    public int CargoShipCount;

    private static GameObject AISpawner = null;
	private static GameObject crimSpawner = null;
	private int waveDistance;
	private int tier;

    public ObjectiveEscortCargo()
    {
        
    }

    public override void SetupObjective(GameObject objectiveManager, int a_tier)
    {
        Position = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));

        if (AISpawner == null)
            AISpawner = Resources.Load("CargoSpawner") as GameObject;
		if(crimSpawner == null)
			crimSpawner = Resources.Load("AISpawner") as GameObject;

        AISpawnerScript spawnerScript = AISpawner.GetComponent<AISpawnerScript>();
		tier = a_tier;
		spawnerScript.tier = tier;
        spawnerScript.maxAI = CargoShipCount;
        spawnerScript.startAI = CargoShipCount;
		
		waveDistance = Random.Range(80, 120);
        AISpawner = (GameObject)GameObject.Instantiate(AISpawner, Position, Quaternion.identity);

		foreach(GameObject g in WarpScript.instance.allPlanets)
		{
			Debug.Log(g.name);
			Debug.Log(objectiveManager.GetComponent<ObjectiveEvent>().objective.sector.ToString());
			if(g.name == objectiveManager.GetComponent<ObjectiveEvent>().objective.sector.name)
			{
				AISpawner.transform.parent = g.transform;
			}
		}
        float xPos = Random.Range(0.01f, 2.0f);
        float yPos = Random.Range(0.01f, 2.0f);
        if (Random.Range(-1.0f, 1.0f) > 0.0f)
        {
            xPos = Mathf.Ceil(xPos);
            // Get xPos to be equal to 1 or 2
            xPos -= 1.0f;
            if (xPos == 0.0f)
                xPos = -1.0f;
            yPos -= 1.0f;
        }
        else
        {
            yPos = Mathf.Ceil(yPos);
            // Get yPos to be equal to 1 or 2
            yPos -= 1.0f;
            if (yPos == 0.0f)
                yPos = -1.0f;
            xPos -= 1.0f;
        }

        Position = new Vector2(xPos, yPos) * 350.0f;
		AISpawner.GetComponent<AISpawnerScript>().Objective = objectiveManager.transform;
        AISpawner.GetComponent<AISpawnerScript>().Init();
    }

    public override void ObjectiveUpdate() 
	{
		if(AISpawner != null && AISpawner.GetComponent<AISpawnerScript>().squad[0] != null)
		{
			Vector2 leadPos = AISpawner.GetComponent<AISpawnerScript>().squad[0].transform.position;
			float cargoDistance = Vector2.Distance(Vector2.zero, leadPos);
			if(Vector2.Distance(leadPos, PlayerShipScript.player.transform.position) < 15.0f)
			{
				if(cargoDistance > waveDistance && waveDistance < 350)
				{
					waveDistance += Random.Range(80, 120);
					float angle = Random.Range(0.0f, 360.0f);
					Vector2 spawnPos = leadPos;
					spawnPos += new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * 50.0f;
					crimSpawner = (GameObject)GameObject.Instantiate(crimSpawner, spawnPos, Quaternion.identity);
					crimSpawner.GetComponent<AISpawnerScript>().tier = tier;
					crimSpawner.GetComponent<AISpawnerScript>().squadLeader = null;
					crimSpawner.GetComponent<AISpawnerScript>().startAI = 1 + tier;
					crimSpawner.GetComponent<AISpawnerScript>().Init();
					foreach(GameObject g in crimSpawner.GetComponent<AISpawnerScript>().squad)
					{
						g.GetComponent<AIShipScript>().Target = AISpawner.GetComponent<AISpawnerScript>().squad[0].transform;
					}
				}
			}
		}
	}

    public override void HitObjective(Collider2D collider)
    {
        if (collider.name.Contains("CargoShip"))
            completed = true;
    }

    protected override JSON ToJSON()
    {
        JSON js = new JSON();
        js["Type"] = "EscortCargo";
        js["CargoShipCount"] = CargoShipCount;

        return js;
    }
    protected override void FromJSON(JSON js)
    {
        CargoShipCount= js.ToInt("CargoShipCount");
    }
}
