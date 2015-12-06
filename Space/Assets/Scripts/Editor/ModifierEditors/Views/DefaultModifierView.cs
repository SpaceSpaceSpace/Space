using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class Modifier
{
    public string Name;
    public float Damage, Accuracy, FireRate;

    public Modifier(string name, float damage, float accuracy, float fireRate)
    {
        //Replace spaces with underscores
        Name = name.Replace(' ', '_');
        Damage = damage;
        Accuracy = accuracy;
        FireRate = fireRate;
    }

    public override string ToString()
    {
        return Name + "," + Damage + "f," + Accuracy + "f," + FireRate + "f";
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
        ModifierEnumifier.LoadAndParseData(out rawData, 1);

        //Loop through raw data and build modifiers
        int rows = rawData.GetLength(0);
        int columns = rawData.GetLength(1);

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
        string filepath = ModifierEnumifier.DATA_PATH;

        //Append CSV of modifier to the end of the data
        if (!File.Exists(filepath))
        {
            Debug.Log("ERROR: Could not find WeaponModifier Data");
            Close();
            return;
        }

        string allData = "ModifierName,DAMAGE,ACCURACY,FIRE_RATE\n";

        for (int i = 0; i < Data.Count; i++)
        {
            Modifier mod = Data[i];

            if (i < Data.Count - 1)
                allData += mod.ToString() + '\n';
            else
                allData += mod.ToString();
        }

        File.WriteAllText(filepath, allData);

        ModifierEnumifier.GenerateFile();
    }
}
