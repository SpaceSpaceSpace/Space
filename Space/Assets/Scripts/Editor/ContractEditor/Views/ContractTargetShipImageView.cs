using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using WyrmTale;

public class ContractTargetShipImageView : ContractViewBase<ContractTargetShipImage>
{
    List<Texture2D> Textures = new List<Texture2D>();

    [MenuItem("Space/View/Contract/Contract Target Ship Image")]
    static void Init()
    {
        ContractTargetShipImageView editor = (ContractTargetShipImageView)GetWindow(typeof(ContractTargetShipImageView));
        editor.minSize = new Vector2(600, 600);
        editor.LoadData();
        editor.InitBase();
        editor.Show();
    }

    protected override void DisplayData(ContractTargetShipImage content)
    {
        int index = Data.IndexOf(content);
        Texture texture = Textures[index];

        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
            GUILayout.Label("Tier: " + content.Tier);
            GUILayout.Label("Target Ship Image Path: " + content.TargetShipImagePath);
            GUILayout.Label(texture, GUILayout.MaxHeight(70), GUILayout.MaxWidth(70));

            ControlsArea(content);
        }
        GUILayout.EndVertical();

        GUILayout.Space(12);
    }

    protected override void LoadData()
    {
        JSON allRawData = ContractUtils.LoadJSONFromFile(ContractElement.ContractElementFilePath);
        JSON[] rawContracts = allRawData.ToArray<JSON>("ContractTargetShipImages");

        Data.Clear();
        Textures.Clear();

        foreach (JSON rawContract in rawContracts)
        {
            ContractTargetShipImage contract = (ContractTargetShipImage)rawContract;
            Texture2D texture = Resources.Load(contract.TargetShipImagePath) as Texture2D;

            Data.Add(contract);
            Textures.Add(texture);
        }
    }

    protected override void WriteData()
    {
        JSON allRawData = ContractUtils.LoadJSONFromFile(ContractElement.ContractElementFilePath);
        int contractCount = Data.Count;

        JSON[] rawContracts = new JSON[contractCount];

        for (int i = 0; i < Data.Count; i++)
            rawContracts[i] = Data[i];

        allRawData["ContractTargetShipImages"] = rawContracts;

        ContractUtils.WriteJSONToFile(ContractElement.ContractElementFilePath, allRawData);
    }
}
