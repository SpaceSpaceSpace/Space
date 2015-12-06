using UnityEngine;
using UnityEditor;
using WyrmTale;
using System.Collections.Generic;

public class ContractContentView : ContractViewBase<ContractContent>
{
    List<Texture2D> TargetShipTextures = new List<Texture2D>();

    [MenuItem("Space/Contracts/Content")]
    static void Init()
    {
        ContractContentView editor = (ContractContentView)GetWindow(typeof(ContractContentView));
        editor.minSize = new Vector2(600, 600);
        editor.LoadData();
        editor.InitBase();
        editor.Show();
    }

    protected override void DisplayData(ContractContent content)
    {
        int index = Data.IndexOf(content);
        Texture texture = TargetShipTextures[index];

        GUILayout.BeginVertical(EditorStyles.helpBox);
        {    
            GUILayout.Label("Tier: " + content.Tier);
            GUILayout.Label("Title: " + content.Title);
            GUILayout.Label("Description: \n" + content.Description);

            GUILayout.Label("Target Ship Image");
            GUILayout.Label(texture, GUILayout.MinHeight(ImagePreviewSize), GUILayout.MaxHeight(ImagePreviewSize), GUILayout.MaxWidth(ImagePreviewSize), GUILayout.MinWidth(ImagePreviewSize));

            if (content.Objectives.Length > 0)
            {
                EditorGUILayout.Space();
                GUILayout.Label("Objectives:");

                foreach (Objective objective in content.Objectives)
                    GUILayout.Label(objective.GetType().ToString() + " - " + objective.sector.name);
            }
            else
            {
                GUILayout.Label("No Objectives :(");
            }

            ControlsArea(content);
        }
        GUILayout.EndVertical();
    
        GUILayout.Space(12);
    }

    protected override void LoadData()
    {
        JSON allRawData = ContractEditorUtils.LoadJSONFromFile(ContractEditorUtils.ContractElementFilePath);
        JSON[] rawContracts = allRawData.ToArray<JSON>("ContractContents");

        Data.Clear();
        TargetShipTextures.Clear();

        foreach (JSON rawContract in rawContracts)
        {
            ContractContent contract = (ContractContent)rawContract;
            Texture2D texture = Resources.Load(contract.TargetShipImagePath) as Texture2D;

            Data.Add(contract);
            TargetShipTextures.Add(texture);
        }
    }

    protected override void WriteData()
    {
        JSON allRawData = ContractEditorUtils.LoadJSONFromFile(ContractEditorUtils.ContractElementFilePath);
        int contractCount = Data.Count;

        JSON[] rawContracts = new JSON[contractCount];
        for (int i = 0; i < Data.Count; i++)
            rawContracts[i] = Data[i];

        allRawData["ContractContents"] = rawContracts;

        ContractEditorUtils.WriteJSONToFile(ContractEditorUtils.ContractElementFilePath, allRawData);
    }
}
