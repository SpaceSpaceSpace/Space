using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using WyrmTale;

public class ContractObjectives : ContractElement
{
    public Objective[] Objectives;

    public ContractObjectives(int Tier, Objective[] Objectives)
    {
        this.Tier = Tier;
        this.Objectives = Objectives;
    }

    //Allows for the conversion from ContractTargetShipImage to JSON for serialization
    public static implicit operator JSON(ContractObjectives objectives)
    {
        JSON js = new JSON();

        if (objectives == null)
            return js;

        js["Tier"] = objectives.Tier;
        js["Objectives"] = objectives.Objectives;

        return js;
    }

    //Allows for the conversion from JSON to ContractTargetShipImage for deserialization
    public static explicit operator ContractObjectives(JSON js)
    {
        checked
        {
            int Tier = js.ToInt("Tier");
            Objective[] Objectives = js.ToArray<Objective>("Objectives");

            return new ContractObjectives(Tier, Objectives);
        }
    }
}