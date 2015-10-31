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
}
