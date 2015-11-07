using UnityEngine;
using WyrmTale;

//Really just a base class that all ContractObjectives will inherit from
public abstract class Objective
{
    //The place that the objective takes place at
    public Vector2 Position;
    public bool Completed {
        get { return completed; }
    }

    protected bool completed;

    //Override to describe what happens during this objective
    public abstract void SetupObjective(GameObject objectiveManager);

    public abstract void ObjectiveUpdate();

    public abstract void HitObjective(Collider2D collider);

    protected abstract JSON ToJSON();
    protected abstract Objective FromJSON(JSON js);

    public static implicit operator JSON(Objective objective)
    {
        return objective.ToJSON();
    }

    //Allows for the conversion from JSON to ContractTargetShipImage for deserialization
    public static explicit operator Objective(JSON js)
    {
        checked
        {
            //Deserialize objective type from JSON

            //Objective obj = new Objective();
            //obj.FromJSON(js);
            return new ObjectiveGoTo();
        }
    }
}
