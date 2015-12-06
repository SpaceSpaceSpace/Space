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
            GUILayout.Label("Name: " + data.Name);
            GUILayout.Label("Damage: " + data.Damage);
            GUILayout.Label("Accuracy: " + data.Accuracy);
            GUILayout.Label("Fire Rate: " + data.FireRate);

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

            float damage = float.Parse(damageString);
            float accuracy = float.Parse(accuracyString);
            float fireRate = float.Parse(fireRateString);

            Data.Add(new Modifier(name, damage, accuracy, fireRate));
        }
    }

    protected override void WriteData()
    {
        ModifierEnumifier.WriteTypeModifiers(2, Data);

        ModifierEnumifier.GenerateFile();
    }
}
