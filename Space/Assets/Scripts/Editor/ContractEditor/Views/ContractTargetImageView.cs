using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using WyrmTale;

public class ContractTargetImageView : ContractViewBase<ContractTargetImage>
{
    List<Texture2D> Textures = new List<Texture2D>();

    [MenuItem("Space/Contracts/Target Image")]
    static void Init()
    {
        ContractTargetImageView editor = (ContractTargetImageView)GetWindow(typeof(ContractTargetImageView));
        editor.minSize = new Vector2(600, 600);
        editor.LoadData();
        editor.InitBase();
        editor.Show();
    }

    protected override void DisplayData(ContractTargetImage content)
    {
        int index = Data.IndexOf(content);
        Texture texture = Textures[index];

        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
            GUILayout.Label("Tier: " + content.Tier);
            GUILayout.Label("Target Image Path: " + content.TargetImagePath);
            GUILayout.Label(texture, GUILayout.MinHeight(ImagePreviewSize), GUILayout.MaxHeight(ImagePreviewSize), GUILayout.MaxWidth(ImagePreviewSize), GUILayout.MinWidth(ImagePreviewSize));

            ControlsArea(content);
        }
        GUILayout.EndVertical();

        GUILayout.Space(12);
    }

    protected override void LoadData()
    {
        JSON allRawData = ContractEditorUtils.LoadJSONFromFile(ContractEditorUtils.ContractElementFilePath);
        JSON[] rawContracts = allRawData.ToArray<JSON>("ContractTargetImages");

        Data.Clear();
        Textures.Clear();

        foreach (JSON rawContract in rawContracts)
        {
            ContractTargetImage contract = (ContractTargetImage)rawContract;
            Texture2D texture = Resources.Load(contract.TargetImagePath) as Texture2D;

            Data.Add(contract);
            Textures.Add(texture);
        }
    }

    protected override void WriteData()
    {
        JSON allRawData = ContractEditorUtils.LoadJSONFromFile(ContractEditorUtils.ContractElementFilePath);
        int contractCount = Data.Count;

        JSON[] rawContracts = new JSON[contractCount];

        for (int i = 0; i < Data.Count; i++)
            rawContracts[i] = Data[i];

        allRawData["ContractTargetImages"] = rawContracts;

        ContractEditorUtils.WriteJSONToFile(ContractEditorUtils.ContractElementFilePath, allRawData);
    }
}
