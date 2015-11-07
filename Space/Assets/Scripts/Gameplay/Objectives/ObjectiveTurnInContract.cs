using UnityEngine;
using WyrmTale;

public class ObjectiveTurnInContract : Objective
{
    Transform store;

    public ObjectiveTurnInContract()
    {
        store = GameObject.Find("SpaceStore").transform;
    }

    public override void SetupObjective(GameObject objectiveManager) { }

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
        return new JSON();
        //throw new NotImplementedException();
    }
    protected override Objective FromJSON(JSON js)
    {
        return new ObjectiveGoTo();
    }
}
