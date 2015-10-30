using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ContractContentView : EditorWindow
{    
    private Vector2 scrollPos;
    
    [MenuItem("Space/View/Contract/Contract Content")]
    static void Init()
    {
        ContractContentView editor = (ContractContentView)GetWindow(typeof(ContractContentView));
        editor.minSize = new Vector2(600, 600);
        ContractContent.LoadContractContents();
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
    
        for (int i = 0; i < ContractContent.ContractContents.Count; i++)
            DisplayContractContent(ContractContent.ContractContents[i]);
    
        EditorGUILayout.EndScrollView();
    
        GUILayout.Space(12);
        GUILayout.FlexibleSpace();
        GUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Data"))
        {
            ContractContent.LoadContractContents();
        }
    
        GUILayout.FlexibleSpace();
    
        if (GUILayout.Button("New Contract Content"))
        {
            ContractContentEditor newContractEditor = ContractContentEditor.Init();
            newContractEditor.OnClose = ReloadContent;
        }
        GUILayout.Space(6);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);
    }
    
    private void DisplayContractContent(ContractContent contractContent)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {    
            GUILayout.Label("Tier: " + contractContent.Tier);
            GUILayout.Label("Title: " + contractContent.Title);
            GUILayout.Label("Description: \n" + contractContent.Description);    
    
            EditorGUILayout.BeginHorizontal();
            {
                //Buttons to move contract up and down
                if (GUILayout.Button("▲"))
                    MoveUp(contractContent);
                GUILayout.Space(6);
    
                if (GUILayout.Button("▼"))
                    MoveDown(contractContent);
                GUILayout.Space(6);
    
                GUILayout.FlexibleSpace();
    
                //Edit and delete buttons in their own horizontal across the bottom
                if (GUILayout.Button("Edit"))
                {
                    ContractContentEditor newContractEditor = ContractContentEditor.Init(contractContent);
                    newContractEditor.OnClose = ReloadContent;
                }
                if (GUILayout.Button("Delete"))
                {
                    if (EditorUtility.DisplayDialog("Deleting Contract Content", "You can't get this back if you delete it. Are you sure you want to delete it?", "Yes I hate this content"))
                    {
                        ContractContent.ContractContents.Remove(contractContent);
                        ContractContent.WriteContractContents();
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
    
    private void ReloadContent()
    {
        ContractContent.LoadContractContents();
        Repaint();
    }
    
    private void MoveUp(ContractContent contractContent)
    {
        int index = ContractContent.ContractContents.IndexOf(contractContent);
        if (index > 0)
        {
            ContractContent.ContractContents.RemoveAt(index);
            ContractContent.ContractContents.Insert(index - 1, contractContent);
    
            Repaint();
            ContractContent.WriteContractContents();
        }
    }
    
    private void MoveDown(ContractContent contract)
    {
        int index = ContractContent.ContractContents.IndexOf(contract);
        if (index < ContractContent.ContractContents.Count - 1)
        {
            ContractContent.ContractContents.RemoveAt(index);
            ContractContent.ContractContents.Insert(index + 1, contract);

            Repaint();
            ContractContent.WriteContractContents();
        }
    }
}
