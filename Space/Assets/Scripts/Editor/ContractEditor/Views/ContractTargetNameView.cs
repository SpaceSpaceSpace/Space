using UnityEngine;
using UnityEditor;
using WyrmTale;

public class ContractTargetNameView : ContractViewBase<ContractTargetName>
{
    private Vector2 scrollPos;

    [MenuItem("Space/View/Contract/Target Name")]
    static void Init()
    {
        ContractTargetNameView editor = (ContractTargetNameView)GetWindow(typeof(ContractTargetNameView));
        editor.minSize = new Vector2(600, 600);
        editor.LoadData();
        editor.InitBase();
        editor.Show();
    }

    protected override void DisplayData(ContractTargetName content)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
            GUILayout.Label("Tier: " + content.Tier);
            GUILayout.Label("Target Name: " + content.TargetName);

            ControlsArea(content);
        }
        GUILayout.EndVertical();

        GUILayout.Space(12);
    }

    protected override void LoadData()
    {
        JSON allRawData = ContractUtils.LoadJSONFromFile(ContractElement.ContractElementFilePath);
        JSON[] rawTargetNames = allRawData.ToArray<JSON>("ContractTargetNames");

        Data.Clear();

        foreach (JSON rawTargetName in rawTargetNames)
        {
            ContractTargetName targetName = (ContractTargetName)rawTargetName;
            Data.Add(targetName);
        }
    }

    protected override void WriteData()
    {
        JSON allRawData = ContractUtils.LoadJSONFromFile(ContractElement.ContractElementFilePath);
        int targetNameCount = Data.Count;

        JSON[] rawTargetNames = new JSON[targetNameCount];
        for (int i = 0; i < Data.Count; i++)
            rawTargetNames[i] = Data[i];

        allRawData["ContractTargetNames"] = rawTargetNames;

        ContractUtils.WriteJSONToFile(ContractElement.ContractElementFilePath, allRawData);
    }
}
