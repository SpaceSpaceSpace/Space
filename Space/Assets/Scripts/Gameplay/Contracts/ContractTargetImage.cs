using WyrmTale;

public class ContractTargetImage : ContractElement
{
    public string TargetImagePath;

    public ContractTargetImage()
    {
        this.Tier = 1;
        this.TargetImagePath = "";
    }

    public ContractTargetImage(int Tier, string TargetImagePath)
    {
        this.Tier = Tier;
        this.TargetImagePath = TargetImagePath;
    }

    //Allows for the conversion from ContractTargetImage to JSON for serialization
    public static implicit operator JSON(ContractTargetImage contract)
    {
        JSON js = new JSON();

        if (contract == null)
            return js;

        js["Tier"] = contract.Tier;
        js["TargetImagePath"] = contract.TargetImagePath;

        return js;
    }

    //Allows for the conversion from JSON to ContractTargetImage for deserialization
    public static explicit operator ContractTargetImage(JSON js)
    {
        checked
        {
            int Tier = js.ToInt("Tier");
            string TargetImagePath = js.ToString("TargetImagePath");

            return new ContractTargetImage(Tier, TargetImagePath);
        }
    }
}
