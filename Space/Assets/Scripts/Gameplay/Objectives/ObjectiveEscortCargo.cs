using UnityEngine;
using WyrmTale;

public class ObjectiveEscortCargo : Objective
{
    private static GameObject AISpawner = null;

    public ObjectiveEscortCargo()
    {
        Position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
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
            AISpawner = Resources.Load("AISpawner") as GameObject;

        Vector2 spawnPos = new Vector2(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
        AISpawner = (GameObject)GameObject.Instantiate(AISpawner, spawnPos, Quaternion.identity);
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
        if (collider.gameObject == PlayerShipScript.player.gameObject)
            completed = true;
    }

    protected override JSON ToJSON()
    {
        return new JSON();
    }
    protected override Objective FromJSON(JSON js)
    {
        return new ObjectiveGoTo();
    }
}
