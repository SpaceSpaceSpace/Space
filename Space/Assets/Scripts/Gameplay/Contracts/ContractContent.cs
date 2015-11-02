using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using WyrmTale;

public class ContractContent : ContractElement
{
    public string Title;
    public string Description;
    
    public ContractContent()
    {
        Tier = 1;
        Title = "";
        Description = "";
    }

    public ContractContent(int Tier, string Title, string Description)
    {
        this.Tier = Tier;
        this.Title = Title;
        this.Description = Description;
    }

    //Allows for the conversion from ContractContent to JSON for serialization
    public static implicit operator JSON(ContractContent contract)
    {
        JSON js = new JSON();

        if (contract == null)
            return js;

        js["Tier"] = contract.Tier;
        js["Title"] = contract.Title;
        js["Description"] = contract.Description;

        return js;
    }

    //Allows for the conversion from JSON to ContractContent for deserialization
    public static explicit operator ContractContent(JSON js)
    {
        checked
        {
            int Tier = js.ToInt("Tier");
            string Title = js.ToString("Title");
            string Description = js.ToString("Description");

            return new ContractContent(Tier, Title, Description);
        }
    }

}