﻿using System;
using WyrmTale;

public class ContractObjectives : ContractElement
{
    public Objective[] Objectives;

    public ContractObjectives(int Tier, Objective[] Objectives)
    {
        this.Tier = Tier;
        this.Objectives = Objectives;
    }

    //Allows for the conversion from ContractObjectives to JSON for serialization
    public static implicit operator JSON(ContractObjectives objectives)
    {
        JSON js = new JSON();

        if (objectives == null)
            return js;

        js["Tier"] = objectives.Tier;
        js["Objectives"] = Array.ConvertAll(objectives.Objectives, item => (JSON)item);

        return js;
    }

    //Allows for the conversion from JSON to ContractObjectives for deserialization
    public static explicit operator ContractObjectives(JSON js)
    {
        checked
        {
            int Tier = js.ToInt("Tier");
            JSON[] rawObjectives = js.ToArray<JSON>("Objectives");
            Objective[] Objectives = Array.ConvertAll(rawObjectives, item => (Objective)item);

            return new ContractObjectives(Tier, Objectives);
        }
    }
}