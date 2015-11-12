﻿using UnityEngine;
using WyrmTale;

public class ObjectiveEscortCargo : Objective
{
    public int CargoShipCount;

    private static GameObject AISpawner = null;

    public ObjectiveEscortCargo()
    {
        Position = new Vector2(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
    }

    public ObjectiveEscortCargo(float minX, float maxX, float minY, float maxY)
    {
        Position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
    }

    public ObjectiveEscortCargo(Vector3 position)
    {
        Position = position;
    }

    public override void SetupObjective(GameObject objectiveManager)
    {
        if (AISpawner == null)
            AISpawner = Resources.Load("CargoSpawner") as GameObject;

        AISpawnerScript spawnerScript = AISpawner.GetComponent<AISpawnerScript>();
        spawnerScript.maxAI = CargoShipCount;
        spawnerScript.startAI = CargoShipCount;

        AISpawner = (GameObject)GameObject.Instantiate(AISpawner, Position, Quaternion.identity);
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

    public override void ObjectiveUpdate() { }

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
