using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public abstract class ContractFormBase : EditorWindow
{
    public delegate void OnCloseEvent();
    public OnCloseEvent OnClose;

    protected const int ImagePreviewSize = 70;

    protected string closeButtonText = "Add";

    protected void ImagePreviewArea(string label, ref string path, ref Texture2D image)
    {
        EditorGUILayout.BeginHorizontal();
        {
            //Display Target image
            GUILayout.Label(image, GUILayout.MinHeight(ImagePreviewSize), GUILayout.MaxHeight(ImagePreviewSize), GUILayout.MaxWidth(ImagePreviewSize), GUILayout.MinWidth(ImagePreviewSize));

            string newPath = EditorGUILayout.TextField(label, path);
            //Check for change
            if (newPath != path)
            {
                path = newPath;
                image = LoadImage(path);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    protected void ObjectiveArea(string label, ref List<ObjectiveType> list)
    {
        int listCount = list.Count;
        int newCount = EditorGUILayout.IntField("Objective Count", listCount);

        ObjectiveType[] array = new ObjectiveType[newCount];
        if (listCount != newCount)
        {
            if (listCount < newCount)
                for (int i = 0; i < listCount; i++)
                    array[i] = list[i];
        }

        for (int i = 0; i < newCount; i++)
        {
            ObjectiveType type = list.ElementAtOrDefault(i);

            type = (ObjectiveType)EditorGUILayout.EnumPopup(type, GUILayout.MaxWidth(150));

            array[i] = type;
        }

        list = array.ToList();
    }

    protected Texture2D LoadImage(string imagePath)
    {
        return Resources.Load(imagePath) as Texture2D;
    }

    //Sets any specific styles we want on editors
    protected void SetEditorStyles()
    {
        EditorStyles.textArea.wordWrap = true;
    }

    void OnDestroy()
    {
        if(OnClose != null)
            OnClose();
    }
}
