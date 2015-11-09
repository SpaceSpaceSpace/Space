using WyrmTale;

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