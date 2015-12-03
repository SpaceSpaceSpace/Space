using UnityEngine;
using UnityEditor;
using System.IO;

public class ModifierForm : ContractFormBase
{
    public string Name;
    public float Damage, Accuracy, FireRate;

    public static ModifierForm Init()
    {
        ModifierForm editor = (ModifierForm)GetWindow(typeof(ModifierForm));
        editor.minSize = new Vector2(300, 100);
        editor.replacementIndex = -1;
        editor.Show();

        return editor;
    }

    public static ModifierForm Init(Modifier modifier, int replacementIndex)
    {
        ModifierForm editor = (ModifierForm)GetWindow(typeof(ModifierForm));
        editor.minSize = new Vector2(300, 100);

        editor.Name = modifier.Name;
        editor.Damage = modifier.Damage;
        editor.Accuracy = modifier.Accuracy;
        editor.FireRate = modifier.FireRate;

        editor.closeButtonText = "Save";
        editor.replacementIndex = replacementIndex;
        editor.Show();

        return editor;
    }

    void OnGUI()
    {
        SetEditorStyles();

        Name = EditorGUILayout.TextField("Name", Name);
        Damage = EditorGUILayout.FloatField("Damage", Damage);
        Accuracy = EditorGUILayout.FloatField("Accuracy", Accuracy);
        FireRate = EditorGUILayout.FloatField("FireRate", FireRate);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(closeButtonText))
                AddContractTargetName(new Modifier(Name, Damage, Accuracy, FireRate));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddContractTargetName(Modifier modifier)
    {
        string filepath = ModifierEnumifier.DATA_PATH;

        //Append CSV of modifier to the end of the data
        if (!File.Exists(filepath))
        {
            Debug.Log("ERROR: Could not find WeaponModifier Data");
            Close();
            return;
        }

        string allData = File.ReadAllText(filepath);
        string[] lines = allData.Replace("\r", "").Split('\n');

        //Test if a modifier with this name exists already
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith(modifier.Name + ','))
            {
                if (EditorUtility.DisplayDialog("Conflict", "A Modifier with this name already exists. Do you want to overwrite?", "Yes", "Oops, Cancel"))
                    replacementIndex = i - 1;
                else
                    return;
            }
        }

        //Just append
        if (replacementIndex < 0)
        {
            allData += "\n" + modifier.ToString();
        } //Insert
        else
        {
            lines[replacementIndex + 1] = modifier.ToString();

            allData = "";
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (i < lines.Length - 1)
                    allData += line + '\n';
                else
                    allData += line;
            }
        }

        File.WriteAllText(filepath, allData);

        ModifierEnumifier.GenerateFile();

        Close();
    }
}
