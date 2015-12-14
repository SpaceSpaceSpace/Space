using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectileModifierForm : ContractFormBase
{
    public string Name;
    public float Damage, Accuracy, FireRate, CostMod;

    public static ProjectileModifierForm Init()
    {
        ProjectileModifierForm editor = (ProjectileModifierForm)GetWindow(typeof(ProjectileModifierForm));
        editor.minSize = new Vector2(300, 100);
        editor.replacementIndex = -1;
        editor.Show();

        return editor;
    }

    public static ProjectileModifierForm Init(Modifier modifier, int replacementIndex)
    {
        ProjectileModifierForm editor = (ProjectileModifierForm)GetWindow(typeof(ProjectileModifierForm));
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
                AddProjectileModifier(new Modifier(Name, Damage, Accuracy, FireRate, CostMod));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddProjectileModifier(Modifier modifier)
    {
        ModifierEnumifier.WriteNewModifier(modifier, replacementIndex, 1); //1 should be the index for Projectile modifiers

        ModifierEnumifier.GenerateFile();

        Close();
    }
}
