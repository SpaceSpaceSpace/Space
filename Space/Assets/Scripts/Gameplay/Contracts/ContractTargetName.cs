using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using WyrmTale;

public class ContractTargetName : ContractElement
{
    public string TargetName;

    public static List<ContractTargetName> ContractTargetNames = new List<ContractTargetName>();

    public ContractTargetName()
    {
        Tier = 1;
        TargetName = "";
    }

    public ContractTargetName(int Tier, string TargetName)
    {
        this.Tier = Tier;
        this.TargetName = TargetName;
    }

    //Allows for the conversion from ContractTargetName to JSON for serialization
    public static implicit operator JSON(ContractTargetName contract)
    {
        JSON js = new JSON();

        if (contract == null)
            return js;

        js["Tier"] = contract.Tier;
        js["TargetName"] = contract.TargetName;

        return js;
    }

    //Allows for the conversion from JSON to ContractContent for deserialization
    public static explicit operator ContractTargetName(JSON js)
    {
        checked
        {
            int Tier = js.ToInt("Tier");
            string TargetName = js.ToString("TargetName");

            return new ContractTargetName(Tier, TargetName);
        }
    }
}
