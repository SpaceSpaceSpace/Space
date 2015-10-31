using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WyrmTale;
using System;

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

        bool replace = false;
        int index = 0;
        for (int i = 0; i < contractContents.Count; i++)
        {
            if (((ContractModel)contractContents[i]).Title == content.Title)
            {
                replace = true;
                index = i;
                break;
            }
        }

        if (replace)
        {
            contractContents.RemoveAt(index);
            contractContents.Insert(index, content);
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
