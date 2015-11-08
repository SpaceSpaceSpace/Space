using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

public abstract class ContractFormBase : EditorWindow
{
    public delegate void OnCloseEvent();
    public OnCloseEvent OnClose;

    protected const int ImagePreviewSize = 70;

    protected string closeButtonText = "Add";
    protected int replacementIndex = -1;

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

    protected void ObjectivesArea(string label, ref List<Objective> list)
    {
        Type[] ObjectiveTypes = GetAllObjectiveTypes();
        string[] ObjectiveStrings = GetAllObjectiveStrings();
        List<string> ObjectiveStringList = ObjectiveStrings.ToList();
        
        int listCount = list.Count;
        int newCount = EditorGUILayout.IntField("Objective Count", listCount);

        Objective[] array = new Objective[newCount];

        if (listCount != newCount)
        {
            if (listCount < newCount)
                for (int i = 0; i < listCount; i++)
                    array[i] = list[i];
        }

        for (int i = 0; i < newCount; i++)
        {
            EditorGUILayout.BeginHorizontal();

            Objective objective = list.ElementAtOrDefault(i);
            if (objective == null)
                objective = new ObjectiveGoTo();

            //Dropdown for the objective type
            Type objectiveType = objective.GetType();
            int currentTypeIndex = ObjectiveStringList.IndexOf(objectiveType.ToString());

            int newIndex = EditorGUILayout.Popup(currentTypeIndex, ObjectiveStrings);
            if (newIndex != currentTypeIndex)
            {
                objectiveType = (ObjectiveTypes[newIndex]);
                objective = (Objective)Activator.CreateInstance(objectiveType);
            }

            ObjectiveArea(ref objective, objectiveType);

            array[i] = objective;

            EditorGUILayout.EndHorizontal();
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
    
    //GUI layout for a specific objective
    private void ObjectiveArea(ref Objective objective, Type derivedType)
    {
        if (derivedType == typeof(ObjectiveGoTo))
        {
            objective.Position = EditorGUILayout.Vector3Field("", objective.Position);
        }
        if (derivedType == typeof(ObjectiveKillTarget))
        {
            ObjectiveKillTarget obj = (ObjectiveKillTarget)objective;
            Debug.Log(obj.GuardCount);
            obj.GuardCount = EditorGUILayout.IntField("Guards", obj.GuardCount);
            objective = obj;
        }
    }

    private Type[] GetAllObjectiveTypes()
    {
        Type[] types = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                        from assemblyType in domainAssembly.GetTypes()
                        where typeof(Objective).IsAssignableFrom(assemblyType)
                        select assemblyType).ToArray();

        return types;
    }

    private string[] GetAllObjectiveStrings()
    {
        Type[] types = GetAllObjectiveTypes();

        string[] strings = new string[types.Length];
        for (int i = 1; i < strings.Length; i++)
            strings[i] = types[i].ToString();

        return strings;
    }


    void OnDestroy()
    {
        if(OnClose != null)
            OnClose();
    }
}
