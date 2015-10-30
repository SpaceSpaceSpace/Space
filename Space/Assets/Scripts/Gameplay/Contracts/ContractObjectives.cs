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