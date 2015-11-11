using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class ContractForm : ContractFormBase
{
    public int Tier = 1;
    public string Title = "";
    public string TargetName = "";
    public string Description = "";
    public string TargetImagePath = "";
    public string TargetShipImagePath = "";
    public List<Objective> Objectives = new List<Objective>();

    private Texture2D TargetImage;
    private Texture2D TargetShipImage;

    public static ContractForm Init()
    {
        ContractForm editor = (ContractForm)GetWindow(typeof(ContractForm));
        editor.minSize = new Vector2(400, 600);
        editor.replacementIndex = -1;
        editor.InternalInit();
        editor.Show();

        return editor;
    }

    public static ContractForm Init(ContractModel existingContract, int replacementIndex)
    {
        ContractForm editor = (ContractForm)GetWindow(typeof(ContractForm));
        editor.minSize = new Vector2(400, 600);
        editor.InternalInit();
        editor.Show();

        editor.Tier = existingContract.Tier;
        editor.Title = existingContract.Title;
        editor.TargetName = existingContract.TargetName;
        editor.Description = existingContract.Description;
        editor.TargetImagePath = existingContract.TargetImagePath;
        editor.TargetShipImagePath = existingContract.TargetShipImagePath;
        editor.Objectives = existingContract.Objectives.ToList();

        editor.replacementIndex = -1;
        editor.closeButtonText = "Save";

        return editor;
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

        EditorGUILayout.LabelField("Objectives");
        ObjectivesArea(ref Objectives);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(closeButtonText))
                AddData();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddData()
    {
        //Reload contracts
        ContractModel.LoadContracts();

        ContractModel model = new ContractModel(Tier, Title, TargetName, Description, TargetImagePath, TargetShipImagePath, Objectives.ToArray());

        if (replacementIndex >= 0)
        {
            ContractModel.Contracts.RemoveAt(replacementIndex);
            ContractModel.Contracts.Insert(replacementIndex, model);
        }
        else
        {
            ContractModel.Contracts.Add(model);
        }

        ContractModel.WriteContracts();

        Close();
    }

}
