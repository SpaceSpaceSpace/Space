using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
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

    protected override ContractElement FromJSON(JSON js)
    {
        ContractContent toCopy = (ContractContent)js;
        Tier = toCopy.Tier;

        return toCopy;
    }

    protected override JSON ToJSON()
    {
        //JSON js = this;
        //return js;

        return null;
    }
}