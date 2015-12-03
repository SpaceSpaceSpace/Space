using UnityEngine;
using WyrmTale;

public class ObjectiveTurnInContract : Objective
{
    GameObject store;

    public override void SetupObjective(GameObject objectiveManager, int tier)
    {
        //if(sector != null)
        //    store = sector.SpaceStore.gameObject;
        //
        //Debug.Log(store);

        //TODO: Make this unnecessary
        if (store == null)
            store = GameObject.Find("SpaceStore");
    }

    public override void ObjectiveUpdate()
    {
        Position = store.transform.position;
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

        return js;
    }
    protected override void FromJSON(JSON js)
    {

    }
}
