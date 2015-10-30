using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Collections.Generic;

public class ContractEditorViewBase<T> : EditorWindow
{
    private Type type;
    private Type editorType;

    private MethodInfo LoadMethod;
    private MethodInfo WriteMethod;
    private FieldInfo Collection;

    private MethodInfo EditorInitMethod;

    protected void InitBase()
    {
        type = typeof(T);
        editorType = Type.GetType(type.ToString() + "Editor");

        LoadMethod = type.GetMethod("Load");
        WriteMethod = type.GetMethod("Write");
        Collection = type.GetField("Data");

        EditorInitMethod = editorType.GetMethod("Init", new Type[] { type });
    }

    protected void SetEditorStyles()
    {
        EditorStyles.textArea.wordWrap = true;
    }

    protected void ReloadContent()
    {
        LoadMethod.Invoke(null, null);
        Repaint();
    }

    protected void MoveUp(T content)
    {
        List<T> collection = Collection.GetValue(null) as List<T>;

        int index = collection.IndexOf(content);
        if (index > 0)
        {
            collection.RemoveAt(index);
            collection.Insert(index - 1, content);

            Repaint();
            WriteMethod.Invoke(null, null);
        }
    }

    protected void MoveDown(T content)
    {
        List<T> collection = Collection.GetValue(null) as List<T>;

        int index = collection.IndexOf(content);
        if (index < collection.Count - 1)
        {
            collection.RemoveAt(index);
            collection.Insert(index + 1, content);

            Repaint();
            WriteMethod.Invoke(null, null);
        }
    }

    protected void ControlsArea(T content)
    {
        List<T> collection = Collection.GetValue(null) as List<T>;

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
                ContractEditorBase genericEditor = EditorInitMethod.Invoke(null, new object[] { content }) as ContractEditorBase;

                genericEditor.OnClose = ReloadContent;
            }
            if (GUILayout.Button("Delete"))
            {
                if (EditorUtility.DisplayDialog("Deleting Content", "You can't get this back if you delete it. Are you sure you want to delete it?", "Yes I hate this"))
                {
                    collection.Remove(content);
                    WriteMethod.Invoke(null, null);
                }
            }
            GUILayout.Space(6);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);
    }

}
