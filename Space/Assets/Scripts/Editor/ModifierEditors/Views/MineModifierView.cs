using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class MineModifierView : ContractViewBase<Modifier>
{
    [MenuItem("Space/Modifiers/Mine")]
    static void Init()
    {
        MineModifierView editor = (MineModifierView)GetWindow(typeof(MineModifierView));
        editor.typeNameOverride = "MineModifierForm";
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
        ModifierEnumifier.LoadAndParseData(out rawData, 3);

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
        ModifierEnumifier.WriteTypeModifiers(3, Data);

        ModifierEnumifier.GenerateFile();
    }
}
