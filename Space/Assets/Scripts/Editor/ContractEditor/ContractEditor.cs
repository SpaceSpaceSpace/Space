using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WyrmTale;

public class ContractEditor : EditorWindow 
{
	public int Tier = 1;
	public string Title = "";
	public string TargetName = "";
	public string Description = "";
	public string TargetImagePath = "";
	public string TargetShipImagePath = "";
    public List<ObjectiveType> Objectives = new List<ObjectiveType>();

    public List<ContractModel> Contracts = new List<ContractModel>();
    public Dictionary<string, Texture2D> ContractTargetImages = new Dictionary<string, Texture2D>();
    public Dictionary<string, Texture2D> ContractTargetShipImages = new Dictionary<string, Texture2D>();

	private Texture2D TargetImage;
	private Texture2D TargetShipImage;

	private const int ImagePreviewSize = 70;
	private const string StoryContractsPath = "Assets/Resources/Contracts/";
	private const string StoryContractsName = "StoryContracts";
	private const string StoryContractsExt = ".json";

    private Vector2 scrollPos;

	[MenuItem("Space/New/Contract/Story Contract")]
	static void Init()
	{
		ContractEditor editor = (ContractEditor)EditorWindow.GetWindow(typeof(ContractEditor));
        editor.minSize = new Vector2(400, 600);
        editor.LoadContracts();
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

        EditorGUILayout.Space();
        GUILayout.Label("New Contract");
        NewContractArea();
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
        }
        GUILayout.EndVertical();

        GUILayout.Space(12);
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
			if(newPath != path)
			{
				path = newPath;
				image = LoadImage (path);
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
            if(listCount < newCount)
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

	private void LoadContracts()
	{
		string contractsContent = "{}";

		try{
			contractsContent = File.ReadAllText(StoryContractsPath + StoryContractsName + StoryContractsExt);
		}catch(FileNotFoundException e){Debug.Log ("Exception: " + e.Message + " " + "Creating new JSON");}

		JSON js = new JSON();
		js.serialized = contractsContent;

        JSON[] rawContracts = js.ToArray<JSON>("Contracts");

        Contracts.Clear();
        foreach (JSON rawContract in rawContracts)
        {
            ContractModel contract = (ContractModel)rawContract;

            Contracts.Add(contract);

            //Pool images so we can display them
            Texture2D targetImage = Resources.Load(contract.TargetImagePath) as Texture2D;
            Texture2D targetShipImage = Resources.Load(contract.TargetShipImagePath) as Texture2D;

            if(targetImage != null)
                ContractTargetImages[contract.TargetImagePath] = targetImage;
            if(targetShipImage != null)
                ContractTargetShipImages[contract.TargetShipImagePath] = targetShipImage;
        }
	}

	private void WriteContracts(string contracts)
	{
		File.WriteAllText(StoryContractsPath + StoryContractsName + StoryContractsExt, contracts);
		AssetDatabase.Refresh();
	}

	private void AddContract()
	{
        bool replace = false;
		int index = 0;
		for(int i = 0; i < Contracts.Count; i++)
		{
			if(((ContractModel)Contracts[i]).Title == Title)
			{
				replace = true;
				index = i;
				break;
			}
		}

		ContractModel model = new ContractModel(Tier, Title, TargetName, Description, TargetImagePath, TargetShipImagePath, Objectives.ToArray());

		if(replace)
		{
			Contracts.RemoveAt(index);
			Contracts.Insert (index, model);
		}
		else
		{
            Contracts.Add(model);
		}

        //Explicitly cast the List of ContractModels to an array of JSON objects
        JSON contractJSON = new JSON();
        JSON[] contractsListJSON = new JSON[Contracts.Count];

        for (int i = 0; i < Contracts.Count; i++)
            contractsListJSON[i] = Contracts[i];

        contractJSON["Contracts"] = contractsListJSON;

		WriteContracts(contractJSON.serialized);

        //Reload contracts
        LoadContracts();
    }
}
