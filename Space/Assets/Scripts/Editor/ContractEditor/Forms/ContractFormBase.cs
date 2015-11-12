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

    //For the objectives 
    private static Dictionary<string, Sector> sectors;
    private static string[] sectorNames;

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

    protected void ObjectivesArea(ref List<Objective> list)
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
                objective = new ObjectiveTurnInContract();

            //Dropdown for the objective type
            Type objectiveType = objective.GetType();
            int currentTypeIndex = ObjectiveStringList.IndexOf(objectiveType.ToString());

            int newIndex = EditorGUILayout.Popup(currentTypeIndex, ObjectiveStrings, GUILayout.Width(150));
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

    protected void InternalInit()
    {
        //Load the sectors into memory for the objectives area
        if (sectors == null)
        {
            sectors = new Dictionary<string, Sector>();

            Sector[] sectorArray = Resources.LoadAll<Sector>("Sectors/");
            sectorNames = new string[sectorArray.Length];

            for(int i = 0; i < sectorArray.Length; i++)
            {
                Sector s = sectorArray[i];

                sectors[s.name] = s;
                sectorNames[i] = s.name;
            }
        }
    }

    //Sets any specific styles we want on editors
    protected void SetEditorStyles()
    {
        EditorStyles.textArea.wordWrap = true;
    }
    
    //GUI layout for a specific objective
    private void ObjectiveArea(ref Objective objective, Type derivedType)
    {
        EditorGUILayout.BeginVertical();

        //If the objective has no given sector, we default to 0, otherwise get the proper index
        int sectorNameIndex = 0;
        if (objective.sector)
            sectorNameIndex = Array.IndexOf(sectorNames, objective.sector.name);
        else 
            objective.sector = sectors[sectorNames[0]]; //Set default sector

        //if a new sector is selected, find it by name in the dictionary and set the proper sector
        int newSectorNameIndex = EditorGUILayout.Popup("Sector", sectorNameIndex, sectorNames);
        if (sectorNameIndex != newSectorNameIndex)
        {
            string sectorName = sectorNames[newSectorNameIndex];
            objective.sector = sectors[sectorName];
        }

        if (derivedType == typeof(ObjectiveKillTarget))
        {
            ObjectiveKillTarget obj = (ObjectiveKillTarget)objective;
            obj.GuardCount = EditorGUILayout.IntField("Guards", obj.GuardCount);
            objective = obj;
        }
        else if (derivedType == typeof(ObjectiveEscortCargo))
        {
            ObjectiveEscortCargo obj = (ObjectiveEscortCargo)objective;
            obj.CargoShipCount = EditorGUILayout.IntField("Cargo Ships", obj.CargoShipCount);
            objective = obj;
        }
        
        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();
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
