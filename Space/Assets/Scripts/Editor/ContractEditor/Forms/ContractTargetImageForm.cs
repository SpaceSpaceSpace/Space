using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using WyrmTale;

public class ContractTargetImageForm : ContractFormBase {

    public int Tier = 1;
    public string TargetImagePath = "";

    private Texture2D TargetImage;

    public static ContractTargetImageForm Init()
    {
        ContractTargetImageForm editor = (ContractTargetImageForm)GetWindow(typeof(ContractTargetImageForm));
        editor.minSize = new Vector2(300, 100);
        editor.replacementIndex = -1;
        editor.Show();

        return editor;
    }

    public static ContractTargetImageForm Init(ContractTargetImage targetImage, int replacementIndex)
    {
        ContractTargetImageForm editor = (ContractTargetImageForm)GetWindow(typeof(ContractTargetImageForm));
        editor.minSize = new Vector2(300, 100);
        editor.Tier = targetImage.Tier;
        editor.TargetImagePath = targetImage.TargetImagePath;
        editor.replacementIndex = replacementIndex;
        editor.closeButtonText = "Save";
        editor.Show();

        return editor;
    }

    void OnGUI()
    {
        SetEditorStyles();

        Tier = EditorGUILayout.IntSlider("Contract Tier", Tier, 1, 10);

        ImagePreviewArea("Target Image", ref TargetImagePath, ref TargetImage);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(closeButtonText))
                AddContractTargetImage(new ContractTargetImage(Tier, TargetImagePath));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddContractTargetImage(ContractTargetImage targetImage)
    {
        string filepath = ContractElement.ContractElementFilePath;

        JSON elementJSON = ContractUtils.LoadJSONFromFile(filepath);

        //Do a bit of deserialization to see if any conflicting contracts exist
        List<JSON> contractTargetImages = elementJSON.ToArray<JSON>("ContractTargetImages").ToList();

        if (replacementIndex >= 0)
        {
            contractTargetImages.RemoveAt(replacementIndex);
            contractTargetImages.Insert(replacementIndex, targetImage);
        }
        else
        {
            contractTargetImages.Add(targetImage);
        }

        elementJSON["ContractTargetImages"] = contractTargetImages;

        ContractUtils.WriteJSONToFile(filepath, elementJSON);

        Close();
    }
}
