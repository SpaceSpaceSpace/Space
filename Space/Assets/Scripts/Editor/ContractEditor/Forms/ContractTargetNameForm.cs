using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WyrmTale;

public class ContractTargetNameForm : ContractFormBase
{
    public int Tier = 1;
    public string TargetName = "";

    public static ContractTargetNameForm Init()
    {
        ContractTargetNameForm editor = (ContractTargetNameForm)GetWindow(typeof(ContractTargetNameForm));
        editor.minSize = new Vector2(300, 100);
        editor.replacementIndex = -1;
        editor.Show();

        return editor;
    }

    public static ContractTargetNameForm Init(ContractTargetName targetName, int replacementIndex)
    {
        ContractTargetNameForm editor = (ContractTargetNameForm)GetWindow(typeof(ContractTargetNameForm));
        editor.minSize = new Vector2(300, 100);
        editor.Tier = targetName.Tier;
        editor.TargetName = targetName.TargetName;
        editor.closeButtonText = "Save";
        editor.replacementIndex = replacementIndex;
        editor.Show();

        return editor;
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
            if (GUILayout.Button(closeButtonText))
                AddContractTargetName(new ContractTargetName(Tier, TargetName));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddContractTargetName(ContractTargetName targetName)
    {
        string filepath = ContractEditorUtils.ContractElementFilePath;
        JSON elementJSON = ContractEditorUtils.LoadJSONFromFile(filepath);

        //Do a bit of deserialization to see if any conflicting contracts exist
        List<JSON> contractTargetNames = elementJSON.ToArray<JSON>("ContractTargetNames").ToList();

        if (replacementIndex >= 0)
        {
            contractTargetNames.RemoveAt(replacementIndex);
            contractTargetNames.Insert(replacementIndex, targetName);
        }
        else
        {
            contractTargetNames.Add(targetName);
        }

        elementJSON["ContractTargetNames"] = contractTargetNames;

        ContractEditorUtils.WriteJSONToFile(filepath, elementJSON);

        Close();
    }
}
