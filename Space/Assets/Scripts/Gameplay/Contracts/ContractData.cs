using UnityEngine;
using System.Collections.Generic;
using WyrmTale;

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
}
public class ContractTargetName : ContractElement
{
	public string TargetName;

	public ContractTargetName(int Tier, string TargetName)
	{
		this.Tier = Tier;
		this.TargetName = TargetName;
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