using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WyrmTale;

public class ContractTargetNameEditor : ContractEditorBase
{
    public int Tier = 1;
    public string TargetName = "";

    private const string ContractContentPath = "Assets/Resources/Contracts/";
    private const string ContractContentName = "ContractElements";
    private const string ContractContentExt = ".json";

    public static ContractTargetNameEditor Init()
    {
        ContractTargetNameEditor editor = (ContractTargetNameEditor)GetWindow(typeof(ContractTargetNameEditor));
        editor.minSize = new Vector2(300, 100);
        editor.Show();

        return editor;
    }

    public static ContractTargetNameEditor Init(ContractTargetName targetName)
    {
        ContractTargetNameEditor editor = (ContractTargetNameEditor)GetWindow(typeof(ContractTargetNameEditor));
        editor.minSize = new Vector2(300, 100);
        editor.Tier = targetName.Tier;
        editor.TargetName = targetName.TargetName;
        editor.Show();

        return editor;
    }

    //Sets any specific styles we want on this editor
    void SetEditorStyles()
    {
        EditorStyles.textArea.wordWrap = true;
    }

    void OnGUI()
    {
        SetEditorStyles();

        Tier = EditorGUILayout.IntSlider("Contract Tier", Tier, 1, 10);

        TargetName = EditorGUILayout.TextField("Target Name", TargetName);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add"))
                AddContractTargetName();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private JSON LoadContractTargetNames()
    {
        string contractsContent = "{}";

        try
        {
            contractsContent = File.ReadAllText(ContractContentPath + ContractContentName + ContractContentExt);
        }
        catch (FileNotFoundException e) { Debug.Log("Exception: " + e.Message + " " + "Creating new JSON"); }

        JSON js = new JSON();
        js.serialized = contractsContent;

        return js;
    }

    private void WriteContractTargetNames(string contracts)
    {
        File.WriteAllText(ContractContentPath + ContractContentName + ContractContentExt, contracts);
        AssetDatabase.Refresh();
    }

    private void AddContractTargetName()
    {
        JSON contractJSON = LoadContractTargetNames();

        //Do a bit of deserialization to see if any conflicting contracts exist
        List<JSON> contractTargetNames = contractJSON.ToArray<JSON>("ContractTargetNames").ToList();

        bool replace = false;
        int index = 0;
        for (int i = 0; i < contractTargetNames.Count; i++)
        {
            if (((ContractModel)contractTargetNames[i]).TargetName == TargetName)
            {
                replace = true;
                index = i;
                break;
            }
        }

        ContractTargetName model = new ContractTargetName(Tier, TargetName);

        if (replace)
        {
            contractTargetNames.RemoveAt(index);
            contractTargetNames.Insert(index, model);
        }
        else
        {
            contractTargetNames.Add(model);
        }

        contractJSON["ContractTargetNames"] = contractTargetNames;

        WriteContractTargetNames(contractJSON.serialized);

        Close();
    }
}
