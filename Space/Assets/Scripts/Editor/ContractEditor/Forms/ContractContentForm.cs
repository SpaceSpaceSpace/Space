using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WyrmTale;
using System;

public class ContractContentForm : ContractFormBase
{
    public int Tier = 1;
    public string Title = "";
    public string Description = "";

    public static ContractContentForm Init()
    {
        ContractContentForm editor = (ContractContentForm)GetWindow(typeof(ContractContentForm));
        editor.minSize = new Vector2(400, 200);
        editor.replacementIndex = -1;
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

        editor.replacementIndex = replacementIndex;
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
                AddContract(new ContractContent(Tier, Title, Description));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    protected void AddContract(ContractContent content)
    {
        string filepath = ContractElement.ContractElementFilePath;

        JSON elementJSON = ContractUtils.LoadJSONFromFile(filepath);

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

        ContractUtils.WriteJSONToFile(filepath, elementJSON);

        Close();
    }
}
