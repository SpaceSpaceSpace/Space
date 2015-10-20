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

	private Texture2D TargetImage;
	private Texture2D TargetShipImage;

	private const int ImagePreviewSize = 70;
	private const string StoryContractsPath = "Assets/Resources/Contracts/";
	private const string StoryContractsName = "StoryContracts";

	[MenuItem("Space/New/Contract/Story Contract")]
	static void Init()
	{
		ContractEditor editor = (ContractEditor)EditorWindow.GetWindow(typeof(ContractEditor));
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

		Tier = EditorGUILayout.IntSlider ("Contract Tier", Tier, 1, 10);

		Title = EditorGUILayout.TextField("Title",Title);
		TargetName = EditorGUILayout.TextField("Target Name",TargetName);

		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Description");
		Description = EditorGUILayout.TextArea(Description, GUILayout.Height(position.height/4));

		ImagePreviewArea ("Target Image Path", ref TargetImagePath, ref TargetImage);

		ImagePreviewArea ("Target Image Ship Path", ref TargetShipImagePath, ref TargetShipImage);

		GUILayout.FlexibleSpace();
		EditorGUILayout.BeginHorizontal();
		{
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Add"))
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

	private Texture2D LoadImage(string imagePath)
	{
		return Resources.Load(imagePath) as Texture2D;
	}

	private JSON LoadContracts()
	{
		TextAsset contractsAsset = Resources.Load(StoryContractsPath + StoryContractsName) as TextAsset;
		string contractsContent = "[]";

		if(contractsAsset != null)
			contractsContent = contractsAsset.text;

		JSON js = new JSON();
		js.serialized = contractsContent;

		return js;
	}

	private void WriteContracts(string contracts)
	{
		File.WriteAllText(StoryContractsPath + StoryContractsName + ".txt", contracts);
		AssetDatabase.Refresh();
	}

	private void AddContract()
	{
		JSON contractJSON = LoadContracts();

		//Do a bit of deserialization to see if any conflicting contracts exist
		List<ContractModel> contracts = contractJSON.ToArray<ContractModel>("Contracts").ToList();

		bool replace = false;
		int index = 0;
		for(int i = 0; i < contracts.Count; i++)
		{
			if(contracts[i].Title == Title)
			{
				replace = true;
				index = i;
				break;
			}
		}

		ContractModel model = new ContractModel(Tier, Title, TargetName, Description, TargetImagePath, TargetShipImagePath);

		if(replace)
		{
			contracts.RemoveAt(index);
			contracts.Insert (index, model);
		}
		else
		{
			contracts.Add(model);
		}

		contractJSON["Contracts"] = (JSON[])contracts.ToArray();

		WriteContracts(contractJSON.serialized);
	}
}
