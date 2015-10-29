using UnityEngine;
using System.Collections.Generic;
using System.IO;
using WyrmTale;

public class ContractData
{
    public static string StoryContractsPath = "Assets/Resources/Contracts/";
    public static string StoryContractsName = "StoryContracts";
    public static string StoryContractsExt = ".json";

    public static void LoadContracts(ref List<ContractModel> Contracts, ref Dictionary<string, Texture2D> ContractTargetImages, ref Dictionary<string, Texture2D> ContractTargetShipImages)
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
}

public abstract class ContractElement
{
    public int Tier;
}
public class ContractContent : ContractElement
{
	public string Title;
    public string Description;

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
public class ContractTargetName : ContractElement
{
	public string TargetName;

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
public class ContractTargetImage : ContractElement
{
	public string TargetImagePath;

	public ContractTargetImage(int Tier, string TargetImagePath)
	{
		this.Tier = Tier;
		this.TargetImagePath = TargetImagePath;
	}
}
public class ContractTargetShipImage : ContractElement
{
	public string TargetShipImagePath;

	public ContractTargetShipImage(int Tier, string TargetShipImagePath)
	{
		this.Tier = Tier;
		this.TargetShipImagePath = TargetShipImagePath;
	}
}
public class ContractRewards : ContractElement
{
    public int SpaceBux;
    //public List<Weapons> RewardWeapons

    public ContractRewards(int Tier, int SpaceBux)
    {
        this.Tier = Tier;
        this.SpaceBux = SpaceBux;
    }
}

public class ContractObjectives : ContractElement
{
    public ObjectiveType[] Objectives;

    public ContractObjectives(int Tier, ObjectiveType[] Objectives)
    {
        this.Tier = Tier;
        this.Objectives = Objectives;
    }
}

public class ContractModel
{
	public int Tier;
	public string Title;
	public string TargetName;
	public string Description;
	public string TargetImagePath;
	public string TargetShipImagePath;
    public ObjectiveType[] Objectives;

	public ContractModel(int Tier, string Title, string TargetName, string Description, string TargetImagePath, string TargetShipImagePath, ObjectiveType[] Objectives)
	{
		this.Tier = Tier;
		this.Title = Title;
		this.TargetName = TargetName;
		this.Description = Description;
		this.TargetImagePath = TargetImagePath;
		this.TargetShipImagePath = TargetShipImagePath;
        this.Objectives = Objectives;
	}

	//Allows for the conversion from ContractModel to JSON for serialization
	public static implicit operator JSON(ContractModel contract)
	{
		JSON js = new JSON();

		if(contract == null)
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
            ObjectiveType[] Objectives = js.ToArray<ObjectiveType>("Objectives");

			return new ContractModel(Tier, Title, TargetName, Description, TargetImagePath, TargetShipImagePath, Objectives);
		}
	}
}