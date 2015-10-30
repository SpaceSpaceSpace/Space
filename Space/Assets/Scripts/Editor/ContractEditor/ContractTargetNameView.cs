using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ContractTargetNameView : EditorWindow
{
    private Vector2 scrollPos;

    [MenuItem("Space/View/Contract/Contract Target Name")]
    static void Init()
    {
        ContractTargetNameView editor = (ContractTargetNameView)GetWindow(typeof(ContractTargetNameView));
        editor.minSize = new Vector2(600, 600);
        ContractTargetName.LoadContractTargetNames();
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

        for (int i = 0; i < ContractTargetName.ContractTargetNames.Count; i++)
            DisplayContractTargetName(ContractTargetName.ContractTargetNames[i]);

        EditorGUILayout.EndScrollView();

        GUILayout.Space(12);
        GUILayout.FlexibleSpace();
        GUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Data"))
        {
            ContractTargetName.LoadContractTargetNames();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("New Contract Target Name"))
        {
            ContractTargetNameEditor newContractEditor = ContractTargetNameEditor.Init();
            newContractEditor.OnClose = ReloadContractTargetNames;
        }
        GUILayout.Space(6);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);
    }

    private void DisplayContractTargetName(ContractTargetName contractTargetName)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
            GUILayout.Label("Tier: " + contractTargetName.Tier);
            GUILayout.Label("Target Name: " + contractTargetName.TargetName);

            EditorGUILayout.BeginHorizontal();
            {
                //Buttons to move data up and down
                if (GUILayout.Button("▲"))
                    MoveUp(contractTargetName);
                GUILayout.Space(6);

                if (GUILayout.Button("▼"))
                    MoveDown(contractTargetName);
                GUILayout.Space(6);

                GUILayout.FlexibleSpace();

                //Edit and delete buttons in their own horizontal across the bottom
                if (GUILayout.Button("Edit"))
                {
                    ContractTargetNameEditor newContractEditor = ContractTargetNameEditor.Init(contractTargetName);
                    newContractEditor.OnClose = ReloadContractTargetNames;
                }
                if (GUILayout.Button("Delete"))
                {
                    if (EditorUtility.DisplayDialog("Deleting Contract Target Name", "You can't get this back if you delete it. Are you sure you want to delete it?", "Yes I hate this content"))
                    {
                        ContractTargetName.ContractTargetNames.Remove(contractTargetName);
                        ContractTargetName.WriteContractTargetNames();
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

    private void ReloadContractTargetNames()
    {
        ContractTargetName.LoadContractTargetNames();
        Repaint();
    }

    private void MoveUp(ContractTargetName contractTargetName)
    {
        int index = ContractTargetName.ContractTargetNames.IndexOf(contractTargetName);
        if (index > 0)
        {
            ContractTargetName.ContractTargetNames.RemoveAt(index);
            ContractTargetName.ContractTargetNames.Insert(index - 1, contractTargetName);

            Repaint();
            ContractTargetName.WriteContractTargetNames();
        }
    }

    private void MoveDown(ContractTargetName contractTargetName)
    {
        int index = ContractTargetName.ContractTargetNames.IndexOf(contractTargetName);
        if (index < ContractTargetName.ContractTargetNames.Count - 1)
        {
            ContractTargetName.ContractTargetNames.RemoveAt(index);
            ContractTargetName.ContractTargetNames.Insert(index + 1, contractTargetName);

            Repaint();
            ContractTargetName.WriteContractTargetNames();
        }
    }
}
