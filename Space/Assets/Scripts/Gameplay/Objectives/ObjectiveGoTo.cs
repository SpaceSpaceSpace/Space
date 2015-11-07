using UnityEngine;
using WyrmTale;

public class ObjectiveGoTo : Objective
{
    public ObjectiveGoTo()
    {
        Position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
    }

    public ObjectiveGoTo(float minX, float maxX, float minY, float maxY)
    {
        Position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
    }

    public ObjectiveGoTo(Vector3 position)
    {
        Position = position;
    }

    public override void SetupObjective(GameObject objectiveManager) { }

    public override void ObjectiveUpdate() { }

    public override void HitObjective(Collider2D collider)
    {
        if(collider.gameObject == PlayerShipScript.player.gameObject)
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
