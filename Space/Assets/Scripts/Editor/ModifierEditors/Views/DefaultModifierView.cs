using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class Modifier
{
    public string Name;
    public float Damage, Accuracy, FireRate, CostMod;

    public Modifier(string name, float damage, float accuracy, float fireRate, float costMod)
    {
        //Replace spaces with underscores
        Name = name.Replace(' ', '_');
        Damage = damage;
        Accuracy = accuracy;
        FireRate = fireRate;
        CostMod = costMod;
    }

    public override string ToString()
    {
        return Name + "," + Damage + "f," + Accuracy + "f," + FireRate + "f," + CostMod + "f";
    }
}

public class DefaultModifierView : ContractViewBase<Modifier>
{
    [MenuItem("Space/Modifiers/Default")]
    static void Init()
    {
        DefaultModifierView editor = (DefaultModifierView)GetWindow(typeof(DefaultModifierView));
        editor.typeNameOverride = "DefaultModifierForm";
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
        ModifierEnumifier.LoadAndParseData(out rawData, 0);

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
        ModifierEnumifier.WriteTypeModifiers(0, Data);

        ModifierEnumifier.GenerateFile();
    }
}
