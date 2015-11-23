using System;
using WyrmTale;

public class ContractContent : ContractElement
{
    public string Title;
    public string Description;
    public string TargetShipImagePath;
    public Objective[] Objectives;
    
    public ContractContent()
    {
        Tier = 1;
        Title = "";
        Description = "";
        TargetShipImagePath = "";
        Objectives = new Objective[2];

        //Default objectives to kill target
        Objectives[0] = new ObjectiveKillTarget();
        Objectives[1] = new ObjectiveTurnInContract();
    }

    public ContractContent(int Tier, string Title, string Description, string TargetShipImagePath, Objective[] Objectives)
    {
        this.Tier = Tier;
        this.Title = Title;
        this.Description = Description;
        this.TargetShipImagePath = TargetShipImagePath;
        this.Objectives = Objectives;
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
        js["TargetShipImagePath"] = contract.TargetShipImagePath;
        js["Objectives"] = Array.ConvertAll(contract.Objectives, item => (JSON)item);

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
            string TargetShipImagePath = js.ToString("TargetShipImagePath");
            JSON[] rawObjectives = js.ToArray<JSON>("Objectives");
            Objective[] Objectives = Array.ConvertAll(rawObjectives, item => (Objective)item);

            return new ContractContent(Tier, Title, Description, TargetShipImagePath, Objectives);
        }
    }

}