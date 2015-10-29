using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class ContractEditor : EditorWindow
{
    public int Tier = 1;
    public string Title = "";
    public string TargetName = "";
    public string Description = "";
    public string TargetImagePath = "";
    public string TargetShipImagePath = "";
    public List<ObjectiveType> Objectives = new List<ObjectiveType>();

    private Texture2D TargetImage;
    private Texture2D TargetShipImage;

    private const int ImagePreviewSize = 70;

    private static List<ContractModel> Contracts = new List<ContractModel>();
    private static Dictionary<string, Texture2D> ContractTargetImages = new Dictionary<string, Texture2D>();
    private static Dictionary<string, Texture2D> ContractTargetShipImages = new Dictionary<string, Texture2D>();

    public delegate void OnCloseEvent();
    public OnCloseEvent OnClose;

    private string buttonText = "Add";

    [MenuItem("Space/New/Contract/Story Contract")]
    public static ContractEditor Init()
    {
        ContractEditor editor = (ContractEditor)GetWindow(typeof(ContractEditor));
        editor.minSize = new Vector2(400, 600);
        editor.Show();

        return editor;
    }

    public static ContractEditor Init(ContractModel existingContract)
    {
        ContractEditor editor = (ContractEditor)GetWindow(typeof(ContractEditor));
        editor.minSize = new Vector2(400, 600);
        editor.Show();

        editor.Tier = existingContract.Tier;
        editor.Title = existingContract.Title;
        editor.TargetName = existingContract.TargetName;
        editor.Description = existingContract.Description;
        editor.TargetImagePath = existingContract.TargetImagePath;
        editor.TargetShipImagePath = existingContract.TargetShipImagePath;
        editor.Objectives = existingContract.Objectives.ToList();

        editor.buttonText = "Edit";

        return editor;
    }

    //Sets any specific styles we want on this editor
    void SetEditorStyles()
    {
        EditorStyles.textArea.wordWrap = true;
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

        ObjectiveArea("Objectives", ref Objectives);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add"))
                AddContract();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void ImagePreviewArea(string label, ref string path, ref Texture2D image)
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

    private void ObjectiveArea(string label, ref List<ObjectiveType> list)
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

    private Texture2D LoadImage(string imagePath)
    {
        return Resources.Load(imagePath) as Texture2D;
    }

    private void AddContract()
    {
        //Reload contracts
        ContractData.LoadContracts(ref Contracts, ref ContractTargetImages, ref ContractTargetShipImages);

        bool replace = false;
        int index = 0;
        for (int i = 0; i < Contracts.Count; i++)
        {
            if ((Contracts[i]).Title == Title)
            {
                replace = true;
                index = i;
                break;
            }
        }

        ContractModel model = new ContractModel(Tier, Title, TargetName, Description, TargetImagePath, TargetShipImagePath, Objectives.ToArray());

        if (replace)
        {
            Contracts.RemoveAt(index);
            Contracts.Insert(index, model);
        }
        else
        {
            Contracts.Add(model);
        }

        ContractData.WriteContracts(Contracts);

        Close();
    }

    void OnDestroy()
    {
        OnClose();
    }

}
