using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using WyrmTale;

public class ContractTargetShipImage : ContractElement
{
    public string TargetShipImagePath;

    public ContractTargetShipImage(int Tier, string TargetShipImagePath)
    {
        this.Tier = Tier;
        this.TargetShipImagePath = TargetShipImagePath;
    }
}
