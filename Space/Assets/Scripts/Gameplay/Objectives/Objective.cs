using UnityEngine;
using System;
using WyrmTale;

//Really just a base class that all ContractObjectives will inherit from
public abstract class Objective
{
    //The place that the objective takes place at
    public Vector2 Position;

    //The Sector the objective takes place in
    public Sector sector;

    public bool Completed {
        get { return completed; }
    }

    protected bool completed;

	public string objectiveType;

    //Override to describe what happens during this objective
    public abstract void SetupObjective(GameObject objectiveManager, int tier = 0);

    public abstract void ObjectiveUpdate();

    public abstract void HitObjective(Collider2D collider);

    protected abstract JSON ToJSON();
    protected abstract void FromJSON(JSON js);

    public static implicit operator JSON(Objective objective)
    {
        // Type specific JSON
        JSON js = objective.ToJSON();

        // Generic JSON
        js["PositionX"] = objective.Position.x;
        js["PositionY"] = objective.Position.y;
        js["Completed"] = objective.completed;
        js["SectorName"] = objective.sector.name;

        return js;
    }

    //Allows for the conversion from JSON to ContractTargetShipImage for deserialization
    public static explicit operator Objective(JSON js)
    {
        checked
        {
            //Deserialize objective type from JSON
            string type = js.ToString("Type");
            Objective objective;

            //Type specific JSON
            switch (type)
            {
                case "TurnInContract":
                    objective = new ObjectiveTurnInContract();
                    break;
                case "KillTarget":
                    objective = new ObjectiveKillTarget();
                    break;
                case "EscortCargo":
                    objective = new ObjectiveEscortCargo();
                    break;
                default:
                    objective = new ObjectiveTurnInContract();
                    break;
            }
            objective.FromJSON(js);

            //Generic JSON
            objective.completed = js.ToBoolean("Completed");
            float x = js.ToFloat("PositionX");
            float y = js.ToFloat("PositionY");
            objective.Position = new Vector2(x, y);

            string sectorName = js.ToString("SectorName");

            //If we're in the editor we can't access the sector names loaded in the ContractFormBase reliably
            //In the application we'll get them from the GameMaster
            if (Application.isEditor)
                objective.sector = Resources.Load<Sector>("Sectors/" + sectorName);
            else
                objective.sector = GameMaster.Sectors[sectorName];

			objective.objectiveType = type;

            return objective;
        }
    }
}
