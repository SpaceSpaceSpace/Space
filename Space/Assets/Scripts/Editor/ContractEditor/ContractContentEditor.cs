using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WyrmTale;

public class ContractContentEditor : EditorWindow
{
    public int Tier = 1;
    public string Title = "";
    public string Description = "";

    private const string ContractContentPath = "Assets/Resources/Contracts/";
    private const string ContractContentName = "ContractElements";
    private const string ContractContentExt = ".json";

    [MenuItem("Space/New/Contract/Contract Content")]
    static void Init()
    {
        ContractContentEditor editor = (ContractContentEditor)EditorWindow.GetWindow(typeof(ContractContentEditor));
        editor.minSize = new Vector2(400, 200);
        editor.Show();
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

        Title = EditorGUILayout.TextField("Title", Title);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Description");
        Description = EditorGUILayout.TextArea(Description, GUILayout.Height(position.height / 4));

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add"))
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
            contractsContent = File.ReadAllText(ContractContentPath + ContractContentName + ContractContentExt);
        }
        catch (FileNotFoundException e) { Debug.Log("Exception: " + e.Message + " " + "Creating new JSON"); }

        JSON js = new JSON();
        js.serialized = contractsContent;

        return js;
    }

    private void WriteContractContent(string contracts)
    {
        File.WriteAllText(ContractContentPath + ContractContentName + ContractContentExt, contracts);
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
