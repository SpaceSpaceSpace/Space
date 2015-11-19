using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using WyrmTale;

public class ContractContentForm : ContractFormBase
{
    public int Tier = 1;
    public string Title = "";
    public string Description = "";
    public List<Objective> Objectives = new List<Objective>();

    public static ContractContentForm Init()
    {
        ContractContentForm editor = (ContractContentForm)GetWindow(typeof(ContractContentForm));
        editor.minSize = new Vector2(400, 200);
        editor.replacementIndex = -1;
        editor.InternalInit();
        editor.Show();

        return editor;
    }

    public static ContractContentForm Init(ContractContent contractContent, int replacementIndex)
    {
        ContractContentForm editor = (ContractContentForm)GetWindow(typeof(ContractContentForm));
        editor.minSize = new Vector2(400, 200);

        editor.Tier = contractContent.Tier;
        editor.Title = contractContent.Title;
        editor.Description = contractContent.Description;
        editor.Objectives = contractContent.Objectives.ToList();

        editor.replacementIndex = replacementIndex;
        editor.closeButtonText = "Save";

        editor.InternalInit();
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

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Objectives");
        ObjectivesArea(ref Objectives);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(closeButtonText))
                AddContract(new ContractContent(Tier, Title, Description, Objectives.ToArray()));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    protected void AddContract(ContractContent content)
    {
        string filepath = ContractEditorUtils.ContractElementFilePath;
        JSON elementJSON = ContractEditorUtils.LoadJSONFromFile(filepath);

        //Do a bit of deserialization to see if any conflicting contracts exist
        List<JSON> contractContents = elementJSON.ToArray<JSON>("ContractContents").ToList();

        if (replacementIndex >= 0)
        {
            contractContents.RemoveAt(replacementIndex);
            contractContents.Insert(replacementIndex, content);
        }
        else
        {
            contractContents.Add(content);
        }

        elementJSON["ContractContents"] = contractContents;

        ContractEditorUtils.WriteJSONToFile(filepath, elementJSON);

        Close();
    }
}
