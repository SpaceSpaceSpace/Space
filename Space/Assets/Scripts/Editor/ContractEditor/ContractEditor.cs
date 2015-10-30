using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class ContractEditor : ContractEditorBase
{
    public int Tier = 1;
    public string Title = "";
    public string TargetName = "";
    public string Description = "";
    public string TargetImagePath = "";
    public string TargetShipImagePath = "";
    public List<ObjectiveType> Objectives = new List<ObjectiveType>();

    private Texture2D TargetImage;
    private Texture2D TargetShipImage;

    public static ContractEditor Init()
    {
        ContractEditor editor = (ContractEditor)GetWindow(typeof(ContractEditor));
        editor.minSize = new Vector2(400, 600);
        editor.Show();

        return editor;
    }

    public static ContractEditor Init(ContractModel existingContract)
    {
        ContractEditor editor = (ContractEditor)GetWindow(typeof(ContractEditor));
        editor.minSize = new Vector2(400, 600);
        editor.Show();

        editor.Tier = existingContract.Tier;
        editor.Title = existingContract.Title;
        editor.TargetName = existingContract.TargetName;
        editor.Description = existingContract.Description;
        editor.TargetImagePath = existingContract.TargetImagePath;
        editor.TargetShipImagePath = existingContract.TargetShipImagePath;
        editor.Objectives = existingContract.Objectives.ToList();

        editor.closeButtonText = "Save";

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

        NewContractArea();
    }

    private void NewContractArea()
    {
        Tier = EditorGUILayout.IntSlider("Contract Tier", Tier, 1, 10);

        Title = EditorGUILayout.TextField("Title", Title);
        TargetName = EditorGUILayout.TextField("Target Name", TargetName);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Description");
        Description = EditorGUILayout.TextArea(Description, GUILayout.Height(position.height / 4));

        ImagePreviewArea("Target Image Path", ref TargetImagePath, ref TargetImage);

        ImagePreviewArea("Target Image Ship Path", ref TargetShipImagePath, ref TargetShipImage);

        EditorGUILayout.Space();

        ObjectiveArea("Objectives", ref Objectives);

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

    private void AddContract()
    {
        //Reload contracts
        ContractModel.LoadContracts();

        bool replace = false;
        int index = 0;
        for (int i = 0; i < ContractModel.Contracts.Count; i++)
        {
            if ((ContractModel.Contracts[i]).Title == Title)
            {
                replace = true;
                index = i;
                break;
            }
        }

        ContractModel model = new ContractModel(Tier, Title, TargetName, Description, TargetImagePath, TargetShipImagePath, Objectives.ToArray());

        if (replace)
        {
            ContractModel.Contracts.RemoveAt(index);
            ContractModel.Contracts.Insert(index, model);
        }
        else
        {
            ContractModel.Contracts.Add(model);
        }

        ContractModel.WriteContracts();

        Close();
    }

}
