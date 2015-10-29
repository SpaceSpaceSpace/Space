using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ContractView : EditorWindow 
{
    private static List<ContractModel> Contracts = new List<ContractModel>();
    private static Dictionary<string, Texture2D> ContractTargetImages = new Dictionary<string, Texture2D>();
    private static Dictionary<string, Texture2D> ContractTargetShipImages = new Dictionary<string, Texture2D>();

    private Vector2 scrollPos;

	[MenuItem("Space/View/Contract/Story Contract")]
	static void Init()
	{
        ContractView editor = (ContractView)GetWindow(typeof(ContractView));
        editor.minSize = new Vector2(600, 600);
        ContractData.LoadContracts(ref Contracts, ref ContractTargetImages, ref ContractTargetShipImages);
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

        for(int i = 0; i < Contracts.Count; i++)
            DisplayContract(Contracts[i]);

        EditorGUILayout.EndScrollView();

        GUILayout.Space(12);
        GUILayout.FlexibleSpace();
        GUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Contracts"))
        {
            ContractData.LoadContracts(ref Contracts, ref ContractTargetImages, ref ContractTargetShipImages);
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

                    if (ContractTargetImages.ContainsKey(targetImagePath))
                    {
                        GUILayout.Label("Target Image - " + targetImagePath);
                        GUILayout.Label(ContractTargetImages[targetImagePath]);
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    string targetShipImagePath = contract.TargetShipImagePath;

                    if (ContractTargetShipImages.ContainsKey(targetShipImagePath))
                    {
                        GUILayout.Label("Target Ship Image - " + targetShipImagePath);
                        GUILayout.Label(ContractTargetShipImages[targetShipImagePath]);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Edit"))
            {
                ContractEditor newContractEditor = ContractEditor.Init(contract);
                newContractEditor.OnClose = ReloadContracts;
            }
            if (GUILayout.Button("Delete"))
            {
                if (EditorUtility.DisplayDialog("Deleting Contract", "You can't get this contract back if you delete it. Are you sure you want to delete it?", "Yes I hate this contract"))
                {
                    Contracts.Remove(contract);
                    ContractData.WriteContracts(Contracts);
                }
            }
            GUILayout.Space(6);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(6);
        }
        GUILayout.EndVertical();

        GUILayout.Space(12);
    }

    private void ReloadContracts()
    {
        ContractData.LoadContracts(ref Contracts, ref ContractTargetImages, ref ContractTargetShipImages);
        Repaint();
    }
}
