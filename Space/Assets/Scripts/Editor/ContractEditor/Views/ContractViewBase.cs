using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Collections.Generic;

public abstract class ContractViewBase<T> : EditorWindow
{
    private MethodInfo EditorInitMethod;
    private MethodInfo EditorInitWithObjectMethod;
    private Vector2 scrollPos;

    private Type type;
    private Type editorType;

    protected List<T> Data = new List<T>();

    protected void InitBase()
    {
        type = typeof(T);
        editorType = Type.GetType(type.ToString() + "Form");

        EditorInitMethod = editorType.GetMethod("Init", new Type[] { });
        EditorInitWithObjectMethod = editorType.GetMethod("Init", new Type[] { type });
    }

    protected void SetEditorStyles()
    {
        EditorStyles.textArea.wordWrap = true;
    }

    protected abstract void DisplayData(T data);
    protected abstract void LoadData();
    protected abstract void WriteData();

    void OnGUI()
    {
        InitBase();

        SetEditorStyles();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < Data.Count; i++)
            DisplayData(Data[i]);

        EditorGUILayout.EndScrollView();

        GUILayout.Space(12);
        GUILayout.FlexibleSpace();
        GUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Data"))
        {
            LoadData();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("New " + type.ToString()))
        {            
            ContractFormBase newEditor = EditorInitMethod.Invoke(null, null) as ContractFormBase;
            newEditor.OnClose = ReloadContent;
        }
        GUILayout.Space(6);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);
    }

    protected void ReloadContent()
    {
        LoadData();
        Repaint();
    }

    protected void MoveUp(T content)
    {
        int index = Data.IndexOf(content);
        if (index > 0)
        {
            Data.RemoveAt(index);
            Data.Insert(index - 1, content);

            Repaint();
            WriteData();
        }
    }

    protected void MoveDown(T content)
    {
        int index = Data.IndexOf(content);
        if (index < Data.Count - 1)
        {
            Data.RemoveAt(index);
            Data.Insert(index + 1, content);

            Repaint();
            WriteData();
        }
    }

    protected void ControlsArea(T content)
    {
        EditorGUILayout.BeginHorizontal();
        {
            //Buttons to move contract up and down
            if (GUILayout.Button("▲"))
                MoveUp(content);
            GUILayout.Space(6);

            if (GUILayout.Button("▼"))
                MoveDown(content);
            GUILayout.Space(6);

            GUILayout.FlexibleSpace();

            //Edit and delete buttons in their own horizontal across the bottom
            if (GUILayout.Button("Edit"))
            {
                ContractFormBase genericEditor = EditorInitWithObjectMethod.Invoke(null, new object[] { content }) as ContractFormBase;

                genericEditor.OnClose = ReloadContent;
            }
            if (GUILayout.Button("Delete"))
            {
                if (EditorUtility.DisplayDialog("Deleting Content", "You can't get this back if you delete it. Are you sure you want to delete it?", "Yes I hate this"))
                {
                    Data.Remove(content);
                    WriteData();
                }
            }
            GUILayout.Space(6);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);
    }

}
