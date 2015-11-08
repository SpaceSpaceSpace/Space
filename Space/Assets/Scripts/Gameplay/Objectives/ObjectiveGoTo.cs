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
        JSON js = new JSON();
        js["Type"] = "GoTo";
        js["PositionX"] = Position.x;
        js["PositionY"] = Position.y;
        js["Completed"] = completed;
        js["Sector"] = 1;

        return js;
    }
    protected override void FromJSON(JSON js)
    {
        completed = js.ToBoolean("Completed");
        float x = js.ToFloat("PositionX");
        float y = js.ToFloat("PositionY");
        Position = new Vector2(x, y);
    }
}
