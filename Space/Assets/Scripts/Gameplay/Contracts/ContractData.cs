using UnityEngine;
using System.Collections;
using WyrmTale;

public abstract class ContractElement
{
	public int Tier;
}

public class ContractTitle : ContractElement
{
	public string Title;

	public ContractTitle(int Tier, string Title)
	{
		this.Tier = Tier;
		this.Title = Title;
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
public class ContractDescription : ContractElement
{
	public string Description;

	public ContractDescription(int Tier, string Description)
	{
		this.Tier = Tier;
		this.Description = Description;
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

public class ContractModel
{
	public int Tier;
	public string Title;
	public string TargetName;
	public string Description;
	public string TargetImagePath;
	public string TargetShipImagePath;

	public ContractModel(int Tier, string Title, string TargetName, string Description, string TargetImagePath, string TargetShipImagePath)
	{
		this.Tier = Tier;
		this.Title = Title;
		this.TargetName = TargetName;
		this.Description = Description;
		this.TargetImagePath = TargetImagePath;
		this.TargetShipImagePath = TargetShipImagePath;
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

			return new ContractModel(Tier, Title, TargetName, Description, TargetImagePath, TargetShipImagePath);
		}
	}
}