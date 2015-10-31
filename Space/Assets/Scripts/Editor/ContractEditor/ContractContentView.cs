using UnityEngine;
using UnityEditor;
using WyrmTale;
using System.Linq;

public class ContractContentView : ContractViewBase<ContractContent>
{        
    [MenuItem("Space/View/Contract/Contract Content")]
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
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {    
            GUILayout.Label("Tier: " + content.Tier);
            GUILayout.Label("Title: " + content.Title);
            GUILayout.Label("Description: \n" + content.Description);

            ControlsArea(content);
        }
        GUILayout.EndVertical();
    
        GUILayout.Space(12);
    }

    protected override void LoadData()
    {
        JSON allRawData = ContractUtils.LoadJSONFromFile(ContractElement.ContractElementFilePath);
        JSON[] rawContracts = allRawData.ToArray<JSON>("ContractContents");

        Data.Clear();

        foreach (JSON rawContract in rawContracts)
        {
            ContractContent contract = (ContractContent)rawContract;
            Data.Add(contract);
        }
    }

    protected override void WriteData()
    {
        JSON allRawData = ContractUtils.LoadJSONFromFile(ContractElement.ContractElementFilePath);
        int contractCount = Data.Count;

        JSON[] rawContracts = new JSON[contractCount];
        for (int i = 0; i < Data.Count; i++)
            rawContracts[i] = Data[i];

        allRawData["ContractContents"] = rawContracts;

        ContractUtils.WriteJSONToFile(ContractElement.ContractElementFilePath, allRawData);
    }
}
