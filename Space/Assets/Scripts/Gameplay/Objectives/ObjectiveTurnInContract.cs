using UnityEngine;
using WyrmTale;

public class ObjectiveTurnInContract : Objective
{
    Transform store;

    public override void SetupObjective(GameObject objectiveManager)
    {
        store = GameObject.Find("SpaceStore").transform;
    }

    public override void ObjectiveUpdate()
    {
        Position = store.position;
    }

    public override void HitObjective(Collider2D collider)
    {
        if (collider.gameObject == PlayerShipScript.player.gameObject)
            completed = true;
    }

    protected override JSON ToJSON()
    {
        JSON js = new JSON();
        js["Type"] = "TurnInContract";
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
