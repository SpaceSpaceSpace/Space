using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using WyrmTale;

public class ContractTargetImage : ContractElement
{
    public string TargetImagePath;

    public ContractTargetImage(int Tier, string TargetImagePath)
    {
        this.Tier = Tier;
        this.TargetImagePath = TargetImagePath;
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
