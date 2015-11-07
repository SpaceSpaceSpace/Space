using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using WyrmTale;

public class ContractModel
{
    public int Tier;
    public string Title;
    public string TargetName;
    public string Description;
    public string TargetImagePath;
    public string TargetShipImagePath;
    public Objective[] Objectives;

    public static string StoryContractsPath = "Assets/Resources/Contracts/";
    public static string StoryContractsName = "StoryContracts";
    public static string StoryContractsExt = ".json";

    public static List<ContractModel> Contracts = new List<ContractModel>();
    public static Dictionary<string, Texture2D> ContractTargetImages = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Texture2D> ContractTargetShipImages = new Dictionary<string, Texture2D>();

    public ContractModel(int Tier, string Title, string TargetName, string Description, string TargetImagePath, string TargetShipImagePath, Objective[] Objectives)
    {
        this.Tier = Tier;
        this.Title = Title;
        this.TargetName = TargetName;
        this.Description = Description;
        this.TargetImagePath = TargetImagePath;
        this.TargetShipImagePath = TargetShipImagePath;
        this.Objectives = Objectives;
    }

    public static void LoadContracts()
    {
        string contractsContent = "{}";

        try
        {
            contractsContent = File.ReadAllText(StoryContractsPath + StoryContractsName + StoryContractsExt);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("Exception: " + e.Message + " " + "Creating new JSON");
        }

        JSON js = new JSON();
        js.serialized = contractsContent;

        JSON[] rawContracts = js.ToArray<JSON>("Contracts");

        Contracts.Clear();
        foreach (JSON rawContract in rawContracts)
        {
            ContractModel contract = (ContractModel)rawContract;

            Contracts.Add(contract);

            //Pool images so we can display them
            Texture2D targetImage = Resources.Load(contract.TargetImagePath) as Texture2D;
            Texture2D targetShipImage = Resources.Load(contract.TargetShipImagePath) as Texture2D;

            if (targetImage != null)
                ContractTargetImages[contract.TargetImagePath] = targetImage;
            if (targetShipImage != null)
                ContractTargetShipImages[contract.TargetShipImagePath] = targetShipImage;
        }
    }

    public static void WriteContracts()
    {
        //Explicitly cast the List of ContractModels to an array of JSON objects
        JSON contractJSON = new JSON();
        JSON[] contractsListJSON = new JSON[Contracts.Count];

        for (int i = 0; i < Contracts.Count; i++)
            contractsListJSON[i] = Contracts[i];

        contractJSON["Contracts"] = contractsListJSON;

        File.WriteAllText(StoryContractsPath + StoryContractsName + StoryContractsExt, contractJSON.serialized);
        AssetDatabase.Refresh();
    }

    //Allows for the conversion from ContractModel to JSON for serialization
    public static implicit operator JSON(ContractModel contract)
    {
        JSON js = new JSON();

        if (contract == null)
            return js;

        js["Tier"] = contract.Tier;
        js["Title"] = contract.Title;
        js["TargetName"] = contract.TargetName;
        js["Description"] = contract.Description;
        js["TargetImagePath"] = contract.TargetImagePath;
        js["TargetShipImagePath"] = contract.TargetShipImagePath;
        js["Objectives"] = contract.Objectives;

        return js;
    }

    //Allows for the conversion from JSON to ContractModel for deserialization
    public static explicit operator ContractModel(JSON js)
    {
        checked
        {
            int Tier = js.ToInt("Tier");
            string Title = js.ToString("Title");
            string TargetName = js.ToString("TargetName");
            string Description = js.ToString("Description");
            string TargetImagePath = js.ToString("TargetImagePath");
            string TargetShipImagePath = js.ToString("TargetShipImagePath");
            Objective[] Objectives = js.ToArray<Objective>("Objectives");

            return new ContractModel(Tier, Title, TargetName, Description, TargetImagePath, TargetShipImagePath, Objectives);
        }
    }
}