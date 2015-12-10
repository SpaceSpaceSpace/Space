using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ScatterModifierView : ContractViewBase<Modifier>
{
    [MenuItem("Space/Modifiers/Scatter")]
    static void Init()
    {
        ScatterModifierView editor = (ScatterModifierView)GetWindow(typeof(ScatterModifierView));
        editor.typeNameOverride = "ScatterModifierForm";
        editor.minSize = new Vector2(600, 600);
        editor.LoadData();
        editor.InitBase();
        editor.Show();
    }

    protected override void DisplayData(Modifier data)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {
            float damagePercent = (data.Damage - 1) * 100;
            string damageString = damagePercent + "%";
            if (damagePercent > 0)
                damageString = "+" + damageString;

            float accuracyPercent = (data.Accuracy - 1) * 100;
            string accuracyString = accuracyPercent + "%";
            if (accuracyPercent > 0)
                accuracyString = "+" + accuracyString;

            float fireRatePercent = (data.FireRate - 1) * 100;
            string fireRateString = fireRatePercent + "%";
            if (fireRatePercent > 0)
                fireRateString = "+" + fireRateString;

            float costModPercent = (data.CostMod - 1) * 100;
            string costModString = costModPercent + "%";
            if (costModPercent > 0)
                costModString = "+" + costModString;

            GUILayout.Label("Name: " + data.Name);
            GUILayout.Label("Damage: " + damageString);
            GUILayout.Label("Accuracy: " + accuracyString);
            GUILayout.Label("Fire Rate: " + fireRateString);
            GUILayout.Label("Cost Modification: " + costModString);

            ControlsArea(data);
        }
        GUILayout.EndVertical();

        GUILayout.Space(12);
    }

    protected override void LoadData()
    {
        Data.Clear();

        string[,] rawData;
        ModifierEnumifier.LoadAndParseData(out rawData, 2);

        //Loop through raw data and build modifiers
        int rows = rawData.GetLength(0);

        for (int i = 0; i < rows; i++)
        {
            string name = rawData[i, 0];

            string damageString = rawData[i, 1].TrimEnd('f');
            string accuracyString = rawData[i, 2].TrimEnd('f');
            string fireRateString = rawData[i, 3].TrimEnd('f');
            string costModString = rawData[i, 4].TrimEnd('f');

            float damage = float.Parse(damageString);
            float accuracy = float.Parse(accuracyString);
            float fireRate = float.Parse(fireRateString);
            float costMod = float.Parse(costModString);

            Data.Add(new Modifier(name, damage, accuracy, fireRate, costMod));
        }
    }

    protected override void WriteData()
    {
        ModifierEnumifier.WriteTypeModifiers(2, Data);

        ModifierEnumifier.GenerateFile();
    }
}
