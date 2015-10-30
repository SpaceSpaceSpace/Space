using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ContractView : EditorWindow 
{
    private Vector2 scrollPos;

	[MenuItem("Space/View/Contract/Story Contract")]
	static void Init()
	{
        ContractView editor = (ContractView)GetWindow(typeof(ContractView));
        editor.minSize = new Vector2(600, 600);
        ContractModel.LoadContracts();
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

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for(int i = 0; i < ContractModel.Contracts.Count; i++)
            DisplayContract(ContractModel.Contracts[i]);

        EditorGUILayout.EndScrollView();

        GUILayout.Space(12);
        GUILayout.FlexibleSpace();
        GUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Contracts"))
        {
            ContractModel.LoadContracts();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("New Contract"))
        {
            ContractEditor newContractEditor = ContractEditor.Init();
            newContractEditor.OnClose = ReloadContracts;
        }
        GUILayout.Space(6);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);
    }

    private void DisplayContract(ContractModel contract)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
            //Controls to move contract up and down
            EditorGUILayout.BeginHorizontal();
            {
                
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Tier: " + contract.Tier);
            GUILayout.Label("Title: " + contract.Title);
            GUILayout.Label("Target Name: " + contract.TargetName);
            GUILayout.Label("Description: \n" + contract.Description);

            //Try to get prefetched images
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    string targetImagePath = contract.TargetImagePath;

                    if (ContractModel.ContractTargetImages.ContainsKey(targetImagePath))
                    {
                        GUILayout.Label("Target Image - " + targetImagePath);
                        GUILayout.Label(ContractModel.ContractTargetImages[targetImagePath]);
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    string targetShipImagePath = contract.TargetShipImagePath;

                    if (ContractModel.ContractTargetShipImages.ContainsKey(targetShipImagePath))
                    {
                        GUILayout.Label("Target Ship Image - " + targetShipImagePath);
                        GUILayout.Label(ContractModel.ContractTargetShipImages[targetShipImagePath]);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            //Display objectives
            if (contract.Objectives.Length > 0)
            {
                EditorGUILayout.Space();
                GUILayout.Label("Objectives:");

                foreach (ObjectiveType objective in contract.Objectives)
                    GUILayout.Label(objective.ToString());
            }
            else
            {
                GUILayout.Label("No Objectives :(");
            }

            
            EditorGUILayout.BeginHorizontal();
            {
                //Buttons to move contract up and down
                if (GUILayout.Button("▲"))
                    MoveUp(contract);
                GUILayout.Space(6);

                if (GUILayout.Button("▼"))
                    MoveDown(contract);
                GUILayout.Space(6);

                GUILayout.FlexibleSpace();

                //Edit and delete buttons in their own horizontal across the bottom
                if (GUILayout.Button("Edit"))
                {
                    ContractEditor newContractEditor = ContractEditor.Init(contract);
                    newContractEditor.OnClose = ReloadContracts;
                }
                if (GUILayout.Button("Delete"))
                {
                    if (EditorUtility.DisplayDialog("Deleting Contract", "You can't get this contract back if you delete it. Are you sure you want to delete it?", "Yes I hate this contract"))
                    {
                        ContractModel.Contracts.Remove(contract);
                        ContractModel.WriteContracts();
                    }
                }
                GUILayout.Space(6);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(6);
        }
        GUILayout.EndVertical();

        GUILayout.Space(12);
    }

    private void ReloadContracts()
    {
        ContractModel.LoadContracts();
        Repaint();
    }

    private void MoveUp(ContractModel contract)
    {
        int index = ContractModel. Contracts.IndexOf(contract);
        if (index > 0)
        {
            ContractModel.Contracts.RemoveAt(index);
            ContractModel.Contracts.Insert(index - 1, contract);

            Repaint();
            ContractModel.WriteContracts();
        }
    }

    private void MoveDown(ContractModel contract)
    {
        int index =  ContractModel.Contracts.IndexOf(contract);
        if (index < ContractModel.Contracts.Count - 1)
        {
             ContractModel.Contracts.RemoveAt(index);
            ContractModel.Contracts.Insert(index + 1, contract);

            Repaint();
            ContractModel.WriteContracts();
        }
    }
}
