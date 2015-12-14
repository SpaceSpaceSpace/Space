using UnityEngine;
using UnityEditor;
using System.IO;

public class ScatterModifierForm : ContractFormBase
{
    public string Name;
    public float Damage, Accuracy, FireRate, CostMod;

    public static ScatterModifierForm Init()
    {
        ScatterModifierForm editor = (ScatterModifierForm)GetWindow(typeof(ScatterModifierForm));
        editor.minSize = new Vector2(300, 100);
        editor.replacementIndex = -1;
        editor.Show();

        return editor;
    }

    public static ScatterModifierForm Init(Modifier modifier, int replacementIndex)
    {
        ScatterModifierForm editor = (ScatterModifierForm)GetWindow(typeof(ScatterModifierForm));
        editor.minSize = new Vector2(300, 100);

        editor.Name = modifier.Name;
        editor.Damage = modifier.Damage;
        editor.Accuracy = modifier.Accuracy;
        editor.FireRate = modifier.FireRate;
        editor.CostMod = modifier.CostMod;

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
        CostMod = EditorGUILayout.FloatField("Cost Mod", CostMod);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(closeButtonText))
                AddScatterModifier(new Modifier(Name, Damage, Accuracy, FireRate, CostMod));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddScatterModifier(Modifier modifier)
    {
        ModifierEnumifier.WriteNewModifier(modifier, replacementIndex, 2); //2 should be the index for default modifiers

        ModifierEnumifier.GenerateFile();

        Close();
    }
}
