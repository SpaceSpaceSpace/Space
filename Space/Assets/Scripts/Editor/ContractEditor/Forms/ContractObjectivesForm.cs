using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using WyrmTale;

public class ContractObjectivesForm : ContractFormBase
{ 
    public int Tier = 1;
    public List<Objective> Objectives = new List<Objective>();

    public static ContractObjectivesForm Init()
    {
        ContractObjectivesForm editor = (ContractObjectivesForm)GetWindow(typeof(ContractObjectivesForm));
        editor.minSize = new Vector2(300, 100);
        editor.replacementIndex = -1;
        editor.Show();

        return editor;
    }

    public static ContractObjectivesForm Init(ContractObjectives objectives, int replacementIndex)
    {
        ContractObjectivesForm editor = (ContractObjectivesForm)GetWindow(typeof(ContractObjectivesForm));
        editor.minSize = new Vector2(300, 100);
        editor.Tier = objectives.Tier;
        editor.Objectives = objectives.Objectives.ToList();
        editor.replacementIndex = replacementIndex;
        editor.closeButtonText = "Save";
        editor.Show();

        return editor;
    }

    void OnGUI()
    {
        SetEditorStyles();

        Tier = EditorGUILayout.IntSlider("Contract Tier", Tier, 1, 10);

        ObjectivesArea("Objectives", ref Objectives);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(closeButtonText))
                AddContractTargetImage(new ContractObjectives(Tier, Objectives.ToArray()));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddContractTargetImage(ContractObjectives objectives)
    {
        string filepath = ContractElement.ContractElementFilePath;

        JSON elementJSON = ContractUtils.LoadJSONFromFile(filepath);

        //Do a bit of deserialization to see if any conflicting contracts exist
        List<JSON> contractObjectives = elementJSON.ToArray<JSON>("ContractObjectives").ToList();

        if (replacementIndex >= 0)
        {
            contractObjectives.RemoveAt(replacementIndex);
            contractObjectives.Insert(replacementIndex, objectives);
        }
        else
        {
            contractObjectives.Add(objectives);
        }

        elementJSON["ContractObjectives"] = contractObjectives;

        ContractUtils.WriteJSONToFile(filepath, elementJSON);

        Close();
    }
}
