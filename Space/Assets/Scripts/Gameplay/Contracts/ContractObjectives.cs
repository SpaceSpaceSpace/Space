using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using WyrmTale;

public class ContractObjectives : ContractElement
{
    public ObjectiveType[] Objectives;

    public ContractObjectives(int Tier, ObjectiveType[] Objectives)
    {
        this.Tier = Tier;
        this.Objectives = Objectives;
    }
}