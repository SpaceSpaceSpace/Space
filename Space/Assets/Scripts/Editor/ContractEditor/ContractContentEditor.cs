using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WyrmTale;

public class ContractContentEditor : ContractEditorBase
{
    public int Tier = 1;
    public string Title = "";
    public string Description = "";

    public static ContractContentEditor Init()
    {
        ContractContentEditor editor = (ContractContentEditor)GetWindow(typeof(ContractContentEditor));
        editor.minSize = new Vector2(400, 200);
        editor.Show();

        return editor;
    }

    public static ContractContentEditor Init(ContractContent contractContent)
    {
        ContractContentEditor editor = (ContractContentEditor)GetWindow(typeof(ContractContentEditor));
        editor.minSize = new Vector2(400, 200);

        editor.Tier = contractContent.Tier;
        editor.Title = contractContent.Title;
        editor.Description = contractContent.Description;

        editor.closeButtonText = "Save";

        editor.Show();

        return editor;
    }

    void OnGUI()
    {
        SetEditorStyles();

        Tier = EditorGUILayout.IntSlider("Contract Tier", Tier, 1, 10);

        Title = EditorGUILayout.TextField("Title", Title);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Description");
        Description = EditorGUILayout.TextArea(Description, GUILayout.Height(position.height / 4));

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(closeButtonText))
                AddContract();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private JSON LoadContractContent()
    {
        string contractsContent = "{}";

        try
        {
            contractsContent = File.ReadAllText(ContractElement.ContractElementPath + ContractElement.ContractElementName + ContractElement.ContractElementExt);
        }
        catch (FileNotFoundException e) { Debug.Log("Exception: " + e.Message + " " + "Creating new JSON"); }

        JSON js = new JSON();
        js.serialized = contractsContent;

        return js;
    }

    private void WriteContractContent(string contracts)
    {
        File.WriteAllText(ContractElement.ContractElementPath + ContractElement.ContractElementName + ContractElement.ContractElementExt, contracts);
        AssetDatabase.Refresh();
    }

    private void AddContract()
    {
        JSON contractJSON = LoadContractContent();

        //Do a bit of deserialization to see if any conflicting contracts exist
        List<JSON> contractContents = contractJSON.ToArray<JSON>("ContractContents").ToList();

        bool replace = false;
        int index = 0;
        for (int i = 0; i < contractContents.Count; i++)
        {
            if (((ContractModel)contractContents[i]).Title == Title)
            {
                replace = true;
                index = i;
                break;
            }
        }

        ContractContent model = new ContractContent(Tier, Title, Description);

        if (replace)
        {
            contractContents.RemoveAt(index);
            contractContents.Insert(index, model);
        }
        else
        {
            contractContents.Add(model);
        }

        contractJSON["ContractContents"] = contractContents;

        WriteContractContent(contractJSON.serialized);

        Close();
    }
}
