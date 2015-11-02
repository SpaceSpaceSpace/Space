using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using WyrmTale;

public class ContractTargetShipImage : ContractElement
{
    public string TargetShipImagePath;

    public ContractTargetShipImage(int Tier, string TargetShipImagePath)
    {
        this.Tier = Tier;
        this.TargetShipImagePath = TargetShipImagePath;
    }

    //Allows for the conversion from ContractTargetShipImage to JSON for serialization
    public static implicit operator JSON(ContractTargetShipImage contract)
    {
        JSON js = new JSON();

        if (contract == null)
            return js;

        js["Tier"] = contract.Tier;
        js["TargetShipImagePath"] = contract.TargetShipImagePath;

        return js;
    }

    //Allows for the conversion from JSON to ContractTargetShipImage for deserialization
    public static explicit operator ContractTargetShipImage(JSON js)
    {
        checked
        {
            int Tier = js.ToInt("Tier");
            string TargetShipImagePath = js.ToString("TargetShipImagePath");

            return new ContractTargetShipImage(Tier, TargetShipImagePath);
        }
    }
}
