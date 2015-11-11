using UnityEngine;
using UnityEditor;
using WyrmTale;

public class ContractObjectivesView : ContractViewBase<ContractObjectives> {

    [MenuItem("Space/View/Contract/Objectives")]
    static void Init()
    {
        ContractObjectivesView editor = (ContractObjectivesView)GetWindow(typeof(ContractObjectivesView));
        editor.minSize = new Vector2(600, 600);
        editor.LoadData();
        editor.InitBase();
        editor.Show();
    }

    protected override void DisplayData(ContractObjectives content)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
            GUILayout.Label("Tier: " + content.Tier);
            GUILayout.Label("Objectives:");

            foreach (Objective objective in content.Objectives)
            {
                GUILayout.Label(objective.GetType().ToString() + " - " + objective.sector.name);
            }
            
            ControlsArea(content);
        }
        GUILayout.EndVertical();

        GUILayout.Space(12);
    }

    protected override void LoadData()
    {
        JSON allRawData = ContractUtils.LoadJSONFromFile(ContractElement.ContractElementFilePath);
        JSON[] rawObjectives = allRawData.ToArray<JSON>("ContractObjectives");

        Data.Clear();

        foreach (JSON rawObjective in rawObjectives)
        {
            ContractObjectives objectives = (ContractObjectives)rawObjective;
            Data.Add(objectives);
        }
    }

    protected override void WriteData()
    {
        JSON allRawData = ContractUtils.LoadJSONFromFile(ContractElement.ContractElementFilePath);
        int objectivesCount = Data.Count;

        JSON[] rawObjectives = new JSON[objectivesCount];
        for (int i = 0; i < Data.Count; i++)
            rawObjectives[i] = Data[i];

        allRawData["ContractObjectives"] = rawObjectives;

        ContractUtils.WriteJSONToFile(ContractElement.ContractElementFilePath, allRawData);
    }
}
