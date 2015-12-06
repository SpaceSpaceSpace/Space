using UnityEngine;
using UnityEditor;
using System.IO;

public class MineModifierForm : ContractFormBase
{
    public string Name;
    public float Damage, Accuracy, FireRate;

    public static MineModifierForm Init()
    {
        MineModifierForm editor = (MineModifierForm)GetWindow(typeof(MineModifierForm));
        editor.minSize = new Vector2(300, 100);
        editor.replacementIndex = -1;
        editor.Show();

        return editor;
    }

    public static MineModifierForm Init(Modifier modifier, int replacementIndex)
    {
        MineModifierForm editor = (MineModifierForm)GetWindow(typeof(MineModifierForm));
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
                AddMineModifier(new Modifier(Name, Damage, Accuracy, FireRate));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddMineModifier(Modifier modifier)
    {
        ModifierEnumifier.WriteNewModifier(modifier, replacementIndex, 3); //0 should be the index for mine modifiers

        ModifierEnumifier.GenerateFile();

        Close();
    }
}
